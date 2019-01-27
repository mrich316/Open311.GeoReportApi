using System;
using Open311.GeoReportApi.OracleEam;
using Open311.GeoReportApi.Services;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddOracleEam(this IServiceCollection services, Action<EamOptions> options)
        {
            services.Configure(options);

            services.AddSingleton<IJurisdictionService, EamJurisdictionService>();
            services.AddSingleton<IServiceStore, EamServiceStore>();
            services.AddSingleton<IServiceRequestSearchService, EamServiceRequestSearchService>();
        }
    }
}
