namespace Open311.GeoReportApi.Services.TestStores
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Models;
    using System.Threading;
    using System.Threading.Tasks;

    public class InMemoryServiceStore : IServiceStore
    {
        private readonly List<Service> _services;

        public InMemoryServiceStore(params Service[] services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            _services = new List<Service>(services);
        }

        public InMemoryServiceStore(IEnumerable<Service> services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            _services = new List<Service>(services);
        }

        public Task<IEnumerable<Service>> GetServices(CancellationToken cancellationToken)
        {
            return Task.FromResult<IEnumerable<Service>>(_services);
        }

        public Task<Service> GetService(string serviceCode, CancellationToken cancellationToken)
        {
            return Task.FromResult(_services.FirstOrDefault(s => s.ServiceCode == serviceCode));
        }

        public async Task<ServiceDefinition> GetServiceDefinition(string serviceCode, CancellationToken cancellationToken)
        {
            var service = await GetService(serviceCode, cancellationToken);

            return service == null
                ? null
                : new ServiceDefinition
                {
                    ServiceCode = serviceCode,
                    ServiceAttributes = service.ServiceAttributes.Values.ToList()
                };
        }
    }
}
