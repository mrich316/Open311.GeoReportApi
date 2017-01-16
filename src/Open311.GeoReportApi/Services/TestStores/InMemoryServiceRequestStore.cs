namespace Open311.GeoReportApi.Services.TestStores
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public class InMemoryServiceRequestStore : IServiceRequestStore
    {
        public Task<ServiceRequestCreated> Create()
        {
            throw new System.NotImplementedException();
        }

        public Task<ServiceRequest> Get(string serviceRequestId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<ServiceRequest>> Search()
        {
            throw new System.NotImplementedException();
        }
    }
}
