namespace Open311.GeoReportApi.Services.TestStores
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Models;
    using System.Threading;
    using System.Threading.Tasks;
    using InputModels;

    public class InMemoryServiceStore : IServiceStore, IServiceRequestSearchService
    {
        private readonly List<Service> _services;
        private readonly Dictionary<string, ServiceRequest> _serviceRequests = new Dictionary<string, ServiceRequest>();

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
                    Attributes = service.Attributes
                };
        }

        public Task<ServiceRequestCreated> Create(PostServiceRequestInputModel serviceRequest)
        {
            var id = Guid.NewGuid().ToString();

            var sr = new ServiceRequest
            {
                Address = serviceRequest.AddressString,
                AddressId = serviceRequest.AddressId,
                Description = serviceRequest.Description,
                Lat = serviceRequest.Lat,
                Long = serviceRequest.Long,
                MediaUrl = serviceRequest.MediaUrl,
                RequestedDatetime = DateTimeOffset.Now,
                ServiceCode = serviceRequest.ServiceCode,
                ServiceRequestId = id,
                Status = ServiceRequestStatus.Open
            };

            _serviceRequests.Add(sr.ServiceRequestId, sr);

            var created = new ServiceRequestCreated
            {
                AccountId = serviceRequest.AccountId,
                ServiceNotice = "We received your service request, thank you.",
                ServiceRequestId = id
            };

            return Task.FromResult(created);
        }

        public Task<IEnumerable<ServiceRequest>> Get(IEnumerable<string> serviceRequestId, CancellationToken cancellationToken)
        {
            var hash = new HashSet<string>(serviceRequestId);

            return Task.FromResult(_serviceRequests
                .Where(kvp => hash.Contains(kvp.Key))
                .Select(kvp => kvp.Value));
        }

        public Task<IEnumerable<ServiceRequest>> Search(ServiceRequestQuery query, CancellationToken cancellationToken)
        {
            List<ServiceRequest> searchResults = null;

            if (query.StartDate.HasValue && query.EndDate.HasValue)
            {
                var source = _serviceRequests.Values.Where(sr =>
                    sr.RequestedDatetime >= query.StartDate &&
                    sr.RequestedDatetime <= query.EndDate);

                searchResults = new List<ServiceRequest>(source);
            }

            if (query.ServiceCodes.Any())
            {
                var source = searchResults != null
                    ? (IEnumerable<ServiceRequest>)searchResults
                    : _serviceRequests.Values;

                searchResults = new List<ServiceRequest>(
                    source.Where(sr => query.ServiceCodes.Contains(sr.ServiceCode)));
            }

            if (query.Statuses.Any())
            {
                var source = searchResults != null
                    ? (IEnumerable<ServiceRequest>)searchResults
                    : _serviceRequests.Values;

                searchResults = new List<ServiceRequest>(
                    source.Where(sr => query.Statuses.Contains(sr.Status)));
            }

            return Task.FromResult<IEnumerable<ServiceRequest>>(searchResults);
        }
    }
}
