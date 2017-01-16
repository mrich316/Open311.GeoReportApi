namespace Open311.GeoReportApi.Services
{
    using System.Threading.Tasks;

    public interface IJurisdictionService
    {
        Task<bool> Exists(string jurisdictionId);

        Task<IServiceStore> GetServiceStore(string jurisdictionId);

        Task<IServiceRequestStore> GetServiceRequestStore(string jurisdictionId);

        Task<IServiceRequestSearchService> GetServiceRequestSearchService(string jurisdictionId);
    }
}
