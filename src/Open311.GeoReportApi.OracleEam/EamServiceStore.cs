using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using Open311.GeoReportApi.InputModels;
using Open311.GeoReportApi.Models;
using Open311.GeoReportApi.Services;
using Oracle.ManagedDataAccess.Client;

namespace Open311.GeoReportApi.OracleEam
{
    public class EamServiceStore : IServiceStore
    {
        private readonly EamOptions _options;

        public EamServiceStore(IOptions<EamOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<IEnumerable<Service>> GetServices(CancellationToken cancellationToken)
        {
            IEnumerable<string> serviceCodes;
            using (var connection = await _options.CreateConnection())
            {
                serviceCodes = await connection.QueryAsync<string>(@"
SELECT lookup_code
FROM fnd_lookup_values_vl
WHERE lookup_type = 'WIP_EAM_WORK_REQ_TYPE'
  AND enabled_flag = 'Y'
ORDER BY lookup_code
");
            }

            var services = await Task.WhenAll(serviceCodes.Select(x => GetService(x, cancellationToken)));

            return services;
        }

        public async Task<Service> GetService(string serviceCode, CancellationToken cancellationToken)
        {
            Service service;
            using (var connection = await _options.CreateConnection())
            {
                service = await connection.QuerySingleOrDefaultAsync<Service>($@"
SELECT lookup_code AS ServiceCode
     , meaning AS ServiceName
     , description AS Description
FROM fnd_lookup_values_vl
WHERE lookup_type = 'WIP_EAM_WORK_REQ_TYPE'
  AND enabled_flag = 'Y'
  AND lookup_code = :{nameof(serviceCode)}
ORDER BY lookup_code
", new {serviceCode});
            }

            service.Type = ServiceType.Realtime;

            // TODO: Fill Group and Keywords

            var serviceDefinition = await GetServiceDefinition(serviceCode, cancellationToken);
            service.Attributes = serviceDefinition.Attributes;

            return service;
        }

        public async Task<ServiceDefinition> GetServiceDefinition(string serviceCode, CancellationToken cancellationToken)
        {
            using (var connection = await _options.CreateConnection())
            {
                var attributes = (await connection.QueryAsync<ServiceAttribute>(@"
SELECT 'true' AS ""Variable""
     , ffcu.end_user_column_name AS ""Code""
     , DECODE(ffvs.validation_type
            , 'F', 'Singlevaluelist'
         , DECODE(ffvs.format_type
            , 'C', 'String'
            , 'N', 'Number'
            , 'N', 'Datetime'
            , 'C'
           )
       ) AS ""Datatype""
     , DECODE(ffcu.required_flag, 'Y', 'True', 'N', 'False', 'N') AS ""Required"" 
     , ffcu.description AS ""DatatypeDescription""
     , ROW_NUMBER() OVER (ORDER BY ffcu.column_seq_num) AS ""Order"" 
FROM fnd_application_vl app
  INNER JOIN fnd_descr_flex_col_usage_vl ffcu ON app.application_id = ffcu.application_id
  INNER JOIN fnd_flex_value_sets ffvs ON ffcu.flex_value_set_id = ffvs.flex_value_set_id
WHERE app.product_code = 'WIP'
  AND ffcu.descriptive_flexfield_name = 'WIP_EAM_WORK_REQUESTS'
  --AND ffcu.descriptive_flex_context_code = 'Global Data Elements'
  AND ffcu.enabled_flag = 'Y'
ORDER BY ffcu.column_seq_num
")).ToList();

                await Task.WhenAll(attributes.Select(AddAttributeValues));

                var serviceDefinition = new ServiceDefinition
                {
                    ServiceCode = serviceCode,
                    Attributes = new ServiceAttributes(attributes)
                };

                return serviceDefinition;
            }
        }

        public async Task<ServiceRequestCreated> Create(PostServiceRequestInputModel serviceRequest)
        {
            var service = await GetService(serviceRequest.ServiceCode, CancellationToken.None);
            if (service == null) throw new ArgumentException($"ServiceCode {serviceRequest.ServiceCode} not found.");

            using (var connection = await _options.CreateConnection())
            {
                try
                {
                    // TODO: The current version of Oracle.ManagedDataAccess.Core Version=2.18.3
                    // does not support oracle objects. We cannot use WIP_EAM_WORK_REQUESTS%ROWTYPE
                    // directly. We had to create an anonymous pl/sql block, but we can not return
                    // any data from it.  Hence, ultra dirty hack, we use RAISE_APPLICATION_ERROR()
                    // to return the work request id.
                    await connection.ExecuteAsync($@"
DECLARE
  l_return_status    VARCHAR2(4000);
  l_msg_count        NUMBER;
  l_msg_data         VARCHAR2(4000); 
  l_mode             VARCHAR2(15) := 'CREATE';
  l_work_request_rec WIP_EAM_WORK_REQUESTS%ROWTYPE;
  l_request_log      VARCHAR2(240) := :{nameof(serviceRequest.Description)};
  l_work_request_id  NUMBER;
BEGIN

  l_work_request_rec.organization_id := :{nameof(serviceRequest.JurisdictionId)};
  l_work_request_rec.work_request_type_id := :{nameof(serviceRequest.ServiceCode)};
  l_work_request_rec.phone_number:= :{nameof(serviceRequest.Phone)};
  l_work_request_rec.e_mail:= :{nameof(serviceRequest.Email)};
  l_work_request_rec.work_request_priority_id := 2;
  l_work_request_rec.work_request_status_id := 2;
  l_work_request_rec.expected_resolution_date := SYSDATE;

  WIP_EAM_WORKREQUEST_PUB.WORK_REQUEST_IMPORT(
    p_api_version      => 1,
    x_return_status    => l_return_status,
    x_msg_count        => l_msg_count,
    x_msg_data         => l_msg_data,
    p_mode             => l_mode,
    p_work_request_rec => l_work_request_rec,
    p_request_log      => l_request_log,
    p_user_id          => :{nameof(serviceRequest.AccountId)},
    x_work_request_id  => l_work_request_id
  );

  IF (l_return_status = 'S')
  THEN
    RAISE_APPLICATION_ERROR(-20000, l_work_request_id);
  ELSE  
    RAISE_APPLICATION_ERROR(-20100, l_msg_data);
  END IF;

  DBMS_OUTPUT.PUT_LINE('l_work_request_id:' || l_work_request_id);
  DBMS_OUTPUT.PUT_LINE('l_return_status: ' || l_return_status);
  DBMS_OUTPUT.PUT_LINE('l_msg_count: ' || l_msg_count);
  DBMS_OUTPUT.PUT_LINE('l_msg_data: ' || l_msg_data);

END;
",
                        new
                        {
                            serviceRequest.AccountId,
                            serviceRequest.Description,
                            serviceRequest.JurisdictionId,
                            serviceRequest.ServiceCode,
                            serviceRequest.Phone,
                            serviceRequest.Email
                        });
                }
                catch (OracleException ex) when (ex.Number == 20000)
                {
                    // TODO: Yeah, pretty dirty and heavy hacking to simulate a function call.
                    // See TODO from above SQL for the reason of this dirt.
                    var serviceRequestId = ex.Message.Substring(11, ex.Message.IndexOf('\n') - 11);
                    return new ServiceRequestCreated {ServiceRequestId = serviceRequestId};
                }

                throw new InvalidOperationException("This method always throws! Wait, what? Yeah. Hacky stuff.");
            }
        }

        private async Task<ServiceAttribute> AddAttributeValues(ServiceAttribute attribute)
        {
            if (attribute.Datatype == ServiceAttributeDatatype.Singlevaluelist
                || attribute.Datatype == ServiceAttributeDatatype.Multivaluelist)
            {
                using (var connection = await _options.CreateConnection())
                {
                    var values = await connection.QueryAsync($@"
SELECT flv.lookup_code AS key
     , flv.meaning AS name
FROM fnd_application_vl app
  INNER JOIN fnd_descr_flex_col_usage_vl ffcu ON app.application_id = ffcu.application_id
  INNER JOIN fnd_flex_value_sets ffvs ON ffcu.flex_value_set_id = ffvs.flex_value_set_id
  INNER JOIN fnd_lookup_values_vl flv ON flv.lookup_type = ffvs.flex_value_set_name
WHERE app.product_code = 'WIP'
  AND ffcu.descriptive_flexfield_name = 'WIP_EAM_WORK_REQUESTS'
  AND ffcu.end_user_column_name = :{nameof(attribute.Code)}
  AND ffcu.enabled_flag = 'Y'
  AND flv.enabled_flag = 'Y'
", new {attribute.Code});

                    attribute.Values = new HashSet<ServiceAttributeValue>(
                        values.Select(x => new ServiceAttributeValue(x.KEY, x.NAME)));
                }
            }

            return attribute;
        }
    }
}
