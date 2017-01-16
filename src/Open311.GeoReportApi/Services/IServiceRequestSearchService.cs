namespace Open311.GeoReportApi.Services
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Models;

    public interface IServiceRequestSearchService
    {
        Task<IEnumerable<ServiceRequest>> Get(IEnumerable<string> serviceRequestId, CancellationToken cancellationToken);

        Task<IEnumerable<ServiceRequest>> Search(ServiceRequestQuery query, CancellationToken cancellationToken);
    }
}
