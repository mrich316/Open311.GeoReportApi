namespace Open311.GeoReportApi.Services.TestStores
{
    using System;
    using System.Threading.Tasks;

    public class InMemoryJurisdictionService : IJurisdictionService
    {
        private readonly IServiceStore _services;
        private readonly IServiceRequestStore _serviceRequests;

        public InMemoryJurisdictionService(IServiceStore services, IServiceRequestStore serviceRequests)
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
    }
}
