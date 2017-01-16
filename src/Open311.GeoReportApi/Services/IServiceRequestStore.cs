namespace Open311.GeoReportApi.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IServiceRequestStore
    {
        // TODO: Create a useful signature.
        Task<ServiceRequestCreated> Create();

        Task<ServiceRequest> Get(string serviceRequestId);

        // TODO: Create a useful signature.
        Task<IEnumerable<ServiceRequest>> Search();
    }
}
