using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using Open311.GeoReportApi.Models;
using Open311.GeoReportApi.Services;

namespace Open311.GeoReportApi.OracleEam
{
    public class EamServiceRequestSearchService : IServiceRequestSearchService
    {
        private static readonly string SqlSelectServiceRequestPart = @"
SELECT wr.work_request_id AS ServiceRequestId
--     , AS Status
--     , AS StatusNotes
     , wr.work_request_type AS ServiceName
     , wr.work_request_type_id AS ServiceCode
     , wr.description
     , wr.work_request_owning_dept AS AgencyResponsible
--     , AS ServiceNotice
     , wr.creation_date AS RequestedDatetime
     , wr.last_update_date AS UpdatedDatetime
--     , AS ExpectedDatetime
--     , AS Address
--     , AS AddressId
--     , AS Zipcode
--     , AS Lat
--     , AS Long
--     , AS MediaUrl
FROM wip_eam_work_requests_v wr
";

        private readonly IOptions<EamOptions> _options;

        public EamServiceRequestSearchService(IOptions<EamOptions> options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<IEnumerable<ServiceRequest>> Get(IEnumerable<string> serviceRequestId, CancellationToken cancellationToken)
        {
            var serviceRequestList = serviceRequestId.ToList();

            if (serviceRequestList.Count == 0)
            {
                return Enumerable.Empty<ServiceRequest>();
            }

            // TODO: Build MaxRecords SQL part (as per specification).
            var sql = $"{SqlSelectServiceRequestPart} WHERE wr.work_request_id IN :{nameof(serviceRequestList)}";

            using (var connection = await _options.Value.CreateConnection())
            {
                var serviceRequests = (await connection.QueryAsync<ServiceRequest>(sql, new { serviceRequestList })).ToList();

                return serviceRequests;
            }

        }

        public async Task<IEnumerable<ServiceRequest>> Search(ServiceRequestQuery query, CancellationToken cancellationToken)
        {
            var whereBuilder = new List<string>();
            var parameters = new DynamicParameters();

            if (query.ServiceCodes != null && query.ServiceCodes.Any())
            {
                whereBuilder.Add($"wr.work_request_type_id IN :{nameof(query.ServiceCodes)}");
                parameters.Add($":{nameof(query.ServiceCodes)}", query.ServiceCodes, DbType.Int32);
            }

            if (query.StartDate.HasValue)
            {
                whereBuilder.Add($"wr.creation_date >= :{nameof(query.StartDate)}");
                parameters.Add($":{nameof(query.StartDate)}", query.StartDate.Value.DateTime, DbType.DateTime);
            }

            if (query.EndDate.HasValue)
            {
                whereBuilder.Add($"wr.creation_date < :{nameof(query.EndDate)}");
                parameters.Add($":{nameof(query.EndDate)}", query.EndDate.Value.DateTime, DbType.Date);
            }

            // TODO: Build where for statuses
            // TODO: Build MaxRecords SQL part (as per specification).

            var sql = whereBuilder.Any()
                ? $"{SqlSelectServiceRequestPart} WHERE {string.Join(" AND ", whereBuilder)}"
                : SqlSelectServiceRequestPart;

            using (var connection = await _options.Value.CreateConnection())
            {
                var serviceRequests = (await connection.QueryAsync<ServiceRequest>(sql, parameters)).ToList();

                return serviceRequests;
            }
        }
    }
}
