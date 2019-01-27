using System;
using System.Globalization;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using Open311.GeoReportApi.Services;

namespace Open311.GeoReportApi.OracleEam
{
    public class EamJurisdictionService : IJurisdictionService
    {
        private readonly IOptions<EamOptions> _options;

        public EamJurisdictionService(IOptions<EamOptions> options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// Current implementation only returns de default EAM org.
        /// </summary>
        /// <param name="jurisdictionId">Represent the organisation id in EAM</param>
        public async Task<bool> Exists(string jurisdictionId)
        {
            using (var connection = await _options.Value.CreateConnection())
            {
                var orgId = await connection.QuerySingleOrDefaultAsync<int>(
                    @"SELECT organization_id FROM wip_eam_parameters_v");

                return (jurisdictionId) == orgId.ToString(CultureInfo.InvariantCulture);
            }
        }

        public Task<IServiceStore> GetServiceStore(string jurisdictionId)
        {
            return Task.FromResult<IServiceStore>(new EamServiceStore(_options));
        }

        public Task<IServiceRequestSearchService> GetServiceRequestSearchService(string jurisdictionId)
        {
            return Task.FromResult<IServiceRequestSearchService>(new EamServiceRequestSearchService(_options));
        }
    }
}
