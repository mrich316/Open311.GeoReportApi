namespace Open311.GeoReportApi.Services
{
    using System.Threading.Tasks;

    public interface IServiceStoreFactory
    {
        Task<bool> JurisdictionExists(string jurisdictionId);

        Task<IServiceStore> GetServiceStore(string jurisdictionId);
    }
}
