namespace Open311.GeoReportApi.Services
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Models;

    public interface IServiceStore
    {
        Task<IEnumerable<Service>> GetServices(CancellationToken cancellationToken);

        Task<object> GetServiceDefinition(string modelServiceCode, CancellationToken cancellationToken);
    }
}
