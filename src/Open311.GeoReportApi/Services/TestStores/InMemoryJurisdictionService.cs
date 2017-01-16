namespace Open311.GeoReportApi.Services.TestStores
{
    using System;
    using System.Threading.Tasks;

    public class InMemoryJurisdictionService : IJurisdictionService
    {
        private readonly InMemoryServiceStore _services;
        private readonly InMemoryServiceRequestStore _serviceRequests;

        public InMemoryJurisdictionService(InMemoryServiceStore services, InMemoryServiceRequestStore serviceRequests)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (serviceRequests == null) throw new ArgumentNullException(nameof(serviceRequests));

            _services = services;
            _serviceRequests = serviceRequests;
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

        public async Task<IServiceRequestStore> GetServiceRequestStore(string jurisdictionId)
        {
            return await Exists(jurisdictionId)
                ? _serviceRequests
                : null;
        }

        public async Task<IServiceRequestSearchService> GetServiceRequestSearchService(string jurisdictionId)
        {
            return await Exists(jurisdictionId)
                ? _serviceRequests
                : null;
        }
    }
}
