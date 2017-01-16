namespace Open311.GeoReportApi.Services
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Models;

    public interface IServiceStore
    {
        Task<IEnumerable<Service>> GetServices(CancellationToken cancellationToken);

        Task<Service> GetService(string serviceCode, CancellationToken cancellationToken);

        Task<ServiceDefinition> GetServiceDefinition(string serviceCode, CancellationToken cancellationToken);
    }
}
