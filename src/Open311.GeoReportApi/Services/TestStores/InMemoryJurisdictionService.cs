namespace Open311.GeoReportApi.Services.TestStores
{
    using System;
    using System.Threading.Tasks;

    public class InMemoryJurisdictionService : IJurisdictionService
    {
        private readonly InMemoryServiceStore _services;

        public InMemoryJurisdictionService(InMemoryServiceStore services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            _services = services;
        }

        public async Task<IServiceStore> GetServiceStore(string jurisdictionId)
        {
            return await Exists(jurisdictionId)
                ? _services
                : null;
        }

        public Task<bool> Exists(string jurisdictionId)
        {
            return Task.FromResult(Open311Options.DefaultJurisdictionId == jurisdictionId);
        }

        public async Task<IServiceRequestSearchService> GetServiceRequestSearchService(string jurisdictionId)
        {
            return await Exists(jurisdictionId)
                ? _services
                : null;
        }
    }
}
