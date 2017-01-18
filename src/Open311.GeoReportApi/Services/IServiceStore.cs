namespace Open311.GeoReportApi.Services
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Threading;
    using System.Threading.Tasks;
    using InputModels;
    using Models;

    public interface IServiceStore
    {
        Task<IEnumerable<Service>> GetServices(CancellationToken cancellationToken);

        Task<Service> GetService(string serviceCode, CancellationToken cancellationToken);

        Task<ServiceDefinition> GetServiceDefinition(string serviceCode, CancellationToken cancellationToken);

        // TODO: Not sure it's a good idea to expose an input model here (PostServiceRequestInputModel).
        Task<ServiceRequestCreated> Create(PostServiceRequestInputModel serviceRequest);
    }
}
