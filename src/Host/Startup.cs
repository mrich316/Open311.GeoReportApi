namespace Host
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Open311.GeoReportApi;
    using Open311.GeoReportApi.Models;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Set a default jurisdiction.
            Open311Options.DefaultJurisdictionId = "city.sdk";

            // Add open 311 preconfigured mvc.
            // It preconfigures output formatters and filters to make it compliant.
            // If you add or change options, you may break compatibility.
            services.AddOpen311Mvc();

            // Add test stores (if not in production).
            services.AddOpen311TestStores(new Service
            {
                Description = "Test service",
                Group = "Group1",
                Keywords = new List<string> {"test", "service"},
                ServiceCode = "test",
                ServiceName = "Test Service",
                Type = ServiceType.Realtime,
                Attributes = new ServiceAttributes
                {
                    new ServiceAttribute
                    {
                        Code = "code",
                        Datatype = ServiceAttributeDatatype.Singlevaluelist,
                        Required = true,
                        Values = new HashSet<ServiceAttributeValue>
                        {
                            new ServiceAttributeValue("a", "Value A"),
                            new ServiceAttributeValue("b", "Value B")
                        }
                    }
                }
            });

            // On a production build, you should provide implementations for
            // open 311 services and add them to the service collection, example:
            //
            //services.AddScoped<IJurisdictionService, YourImplementationJurisdictionService>();
            //services.AddScoped<IServiceStore, YourImplementationServiceStore>();
            //services.AddScoped<IServiceRequestSearchService, YourImplementationServiceRequestSearchService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvcWithDefaultRoute();
        }
    }
}
