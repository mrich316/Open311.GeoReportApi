// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    using System;
    using AspNetCore.Mvc.ModelBinding;
    using Net.Http.Headers;
    using Newtonsoft.Json.Serialization;
    using Open311.GeoReportApi;
    using Open311.GeoReportApi.Filters;
    using Open311.GeoReportApi.ModelBinding;
    using Open311.GeoReportApi.Models;
    using Open311.GeoReportApi.Services;
    using Open311.GeoReportApi.Services.TestStores;

    public static class MvcServiceCollectionExtensions
    {
        public static IMvcBuilder AddOpen311Mvc(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSingleton<IServiceAttributeValidator, DefaultServiceAttributeValidator>();

            return services.AddMvc()
                .AddMvcOptions(options =>
                {
                    // Add xml output formatter and mapping.
                    options.OutputFormatters.Add(Open311Options.CreateXmlSerializerOutputFormatter());

                    options.FormatterMappings.SetMediaTypeMappingForFormat("xml",
                        new MediaTypeHeaderValue("application/xml"));

                    // Replace value providers with snake_case variants.
                    var namingStrategy = new SnakeCaseNamingStrategy(true, false);
                    options.ValueProviderFactories.AddOrReplace<IValueProviderFactory, QueryStringValueProviderFactory>(
                        new NamingStrategyQueryStringValueProviderFactory(namingStrategy));

                    options.ValueProviderFactories.AddOrReplace<IValueProviderFactory, FormValueProviderFactory>(
                        new NamingStrategyFormValueProviderFactory(namingStrategy));

                    // Add action validators
                    options.Filters.Add(typeof(ValidateModelStateAttribute));
                    options.Filters.Add(typeof(ValidateJurisdictionAttribute));
                })
                .AddJsonOptions(Open311Options.SetupJsonOptions);
        }

        public static IServiceCollection AddOpen311TestStores(this IServiceCollection services, params Service[] open311Services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSingleton<IJurisdictionService, InMemoryJurisdictionService>();
            services.AddSingleton(provider => new InMemoryServiceStore(open311Services));

            return services;
        }
    }
}
