namespace Open311.GeoReportApi.Services
{
    using System.Threading.Tasks;
    using Models;

    public interface IServiceRequestStore
    {
        // TODO: Create a useful signature.
        Task<ServiceRequestCreated> Create();
    }
}
