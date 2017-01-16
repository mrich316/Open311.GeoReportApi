namespace Open311.GeoReportApi.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IServiceRequestSearchService
    {
        Task<ServiceRequest> Get(string serviceRequestId);

        Task<IEnumerable<ServiceRequest>> Get(IEnumerable<string> serviceRequestId);

        Task<IEnumerable<ServiceRequest>> Search(ServiceRequestQuery query);
    }
}
