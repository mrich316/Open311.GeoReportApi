 // ReSharper disable once CheckNamespace
namespace Open311.GeoReportApi.Services
{
    using System.Threading.Tasks;
    using InputModels;

    public static class JurisdictionServiceExtensions
    {
        public static Task<IServiceStore> GetServiceStore<T>(this IJurisdictionService jurisdiction, T model)
            where T : BaseInputModel
        {
            return jurisdiction.GetServiceStore(model.JurisdictionId);
        }

        public static Task<IServiceRequestSearchService> GetServiceRequestSearchService<T>(this IJurisdictionService jurisdiction, T model)
            where T : BaseInputModel
        {
            return jurisdiction.GetServiceRequestSearchService(model.JurisdictionId);
        }
    }
}
