namespace Open311.GeoReportApi.Services.TestStores
{
    using System;
    using System.Threading.Tasks;

    public class InMemoryServiceStoreFactory : IServiceStoreFactory
    {
        private readonly IServiceStore _serviceStore;

        public InMemoryServiceStoreFactory(IServiceStore serviceStore)
        {
            if (serviceStore == null) throw new ArgumentNullException(nameof(serviceStore));
            _serviceStore = serviceStore;
        }

        public async Task<IServiceStore> GetServiceStore(string jurisdictionId)
        {
            return await JurisdictionExists(jurisdictionId)
                ? _serviceStore
                : null;
        }

        public Task<bool> JurisdictionExists(string jurisdictionId)
        {
            return Task.FromResult(Open311Options.DefaultJurisdictionId == jurisdictionId);
        }
    }
}
