namespace Open311.GeoReportApi
{
    using System.Collections.Generic;
    using Filters;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
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
                    options.OutputFormatters.Add(CreateXmlSerializerOutputFormatter());

                    options.FormatterMappings.SetMediaTypeMappingForFormat("xml",
                        new MediaTypeHeaderValue("application/xml"));

                    // Replace value providers with snake_case variants.
                    options.ValueProviderFactories.AddOrReplace<IValueProviderFactory, QueryStringValueProviderFactory>(
                        new NamingStrategyQueryStringValueProviderFactory(namingStrategy));

                    options.ValueProviderFactories.AddOrReplace<IValueProviderFactory, FormValueProviderFactory>(
                        new NamingStrategyFormValueProviderFactory(namingStrategy));

                    // Add action validators
                    options.Filters.Add(typeof(ValidateModelStateAttribute));
                    options.Filters.Add(typeof(ValidateJurisdictionAttribute));
                })
                .AddJsonOptions(SetupJsonOptions);

            services.AddSingleton<IJurisdictionService, InMemoryJurisdictionService>();
            services.AddSingleton<IServiceAttributeValidator, DefaultServiceAttributeValidator>();
            services.AddSingleton(provider => new InMemoryServiceStore(
                new Service
                {
                    Description = "Test service",
                    Group = "Envir",
                    Keywords = "asd,asd",
                    ServiceCode = "ENV-TO",
                    ServiceName = "Environnement",
                    Type = ServiceType.Realtime,
                    Attributes = new ServiceAttributes
                    {
                        new ServiceAttribute
                        {
                            Code = "100",
                            Datatype = ServiceAttributeDatatype.Singlevaluelist,
                            Required = true,
                            Values = new HashSet<ServiceAttributeValue>
                            {
                                new ServiceAttributeValue("a"),
                                new ServiceAttributeValue("b")
                            }
                        }
                    }
                }
            ));
        }

        public void SetupJsonOptions(MvcJsonOptions options)
        {
            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy(true, false)
            };

            options.SerializerSettings.ContractResolver = contractResolver;
            options.SerializerSettings.Formatting = Formatting.Indented;
            options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            options.SerializerSettings.Converters.Add(new StringEnumConverter(true));
        }

        public XmlDataContractSerializerOutputFormatter CreateXmlSerializerOutputFormatter()
        {
            return new XmlDataContractSerializerOutputFormatter(
                new System.Xml.XmlWriterSettings {Indent = true})
            {
                SerializerSettings = {SerializeReadOnlyTypes = true}
            };
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