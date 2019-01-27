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

        public Task<ServiceRequestCreated> Create(PostServiceRequestInputModel serviceRequest)
        {
            throw new System.NotImplementedException();
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
