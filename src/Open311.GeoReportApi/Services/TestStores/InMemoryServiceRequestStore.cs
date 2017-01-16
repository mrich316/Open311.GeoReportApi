namespace Open311.GeoReportApi.Services.TestStores
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;
    using System.Linq;

    public class InMemoryServiceRequestStore : IServiceRequestStore, IServiceRequestSearchService
    {
        private Dictionary<string, ServiceRequest> _serviceRequests = new Dictionary<string, ServiceRequest>();

        public Task<ServiceRequestCreated> Create()
        {
            throw new System.NotImplementedException();
        }

        public Task<ServiceRequest> Get(string serviceRequestId)
        {
            return Task.FromResult(_serviceRequests
                .FirstOrDefault(kvp => kvp.Key == serviceRequestId).Value);
        }

        public Task<IEnumerable<ServiceRequest>> Get(IEnumerable<string> serviceRequestId)
        {
            var hash = new HashSet<string>(serviceRequestId);

            return Task.FromResult(_serviceRequests
                .Where(kvp => hash.Contains(kvp.Key))
                .Select(kvp => kvp.Value));
        }

        public Task<IEnumerable<ServiceRequest>> Search(ServiceRequestQuery query)
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
                    ? (IEnumerable<ServiceRequest>) searchResults
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
