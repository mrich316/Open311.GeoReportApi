namespace Open311.GeoReportApi.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IServiceStore
    {
        Task<IEnumerable<Service>> GetServices(string jurisdictionId);
    }
}
