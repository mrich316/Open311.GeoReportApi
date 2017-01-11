namespace Open311.GeoReportApi
{
    using System.Linq;
    using Filters;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Net.Http.Headers;
    using ModelBinding;
    using Models;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;
    using Services;
    using Services.TestStores;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var namingStrategy = new SnakeCaseNamingStrategy(true, false);

            services.AddMvc()
                .AddMvcOptions(options =>
                {
                    // Add xml output formatter and mapping.
                    options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter(
                        new System.Xml.XmlWriterSettings {Indent = true}));

                    options.FormatterMappings.SetMediaTypeMappingForFormat("xml",
                        new MediaTypeHeaderValue("application/xml"));

                    // Replace QueryStringValueProvider with a snake_case variant.
                    var queryProvider =
                        options.ValueProviderFactories.FirstOrDefault(
                            f => f.GetType() == typeof(QueryStringValueProviderFactory));

                    if (queryProvider != null)
                    {
                        var index = options.ValueProviderFactories.IndexOf(queryProvider);
                        options.ValueProviderFactories.Insert(index,
                            new NamingStrategyQueryStringValueProviderFactory(namingStrategy));
                        options.ValueProviderFactories.Remove(queryProvider);
                    }
                    else
                    {
                        options.ValueProviderFactories.Add(
                            new NamingStrategyQueryStringValueProviderFactory(namingStrategy));
                    }

                    // Add action validators
                    options.Filters.Add(typeof(ValidateAttribute));
                })
                .AddJsonOptions(options =>
                {
                    var contractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = namingStrategy
                    };

                    options.SerializerSettings.ContractResolver = contractResolver;
                    options.SerializerSettings.Formatting = Formatting.Indented;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter(true));
                });

            services.AddScoped<IServiceStore>(provider => new InMemoryServiceStore(
                new Service
                {
                    Description = "Test service",
                    Group = "Envir",
                    Keywords = "asd,asd",
                    ServiceCode = "ENV-TO",
                    Metadata = false,
                    ServiceName = "Environnement",
                    Type = ServiceType.Realtime
                }
            ));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvcWithDefaultRoute();
        }
    }
}