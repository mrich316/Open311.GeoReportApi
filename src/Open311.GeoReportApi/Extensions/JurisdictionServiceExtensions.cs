 // ReSharper disable once CheckNamespace
namespace Open311.GeoReportApi.Services
{
    using System.Threading.Tasks;
    using InputModels;
    using Services;

    public static class JurisdictionServiceExtensions
    {
        public static Task<IServiceStore> GetServiceStore<T>(this IJurisdictionService jurisdiction, T model)
            where T : BaseInputModel
        {
            return jurisdiction.GetServiceStore(model.JurisdictionId);
        }

        public static Task<IServiceRequestStore> GetServiceRequestStore<T>(this IJurisdictionService jurisdiction, T model)
            where T : BaseInputModel
        {
            return jurisdiction.GetServiceRequestStore(model.JurisdictionId);
        }
    }
}
