namespace Open311.GeoReportApi.Tests
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;
    using GeoReportApi.Controllers;
    using GeoReportApi.InputModels;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Moq;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;
    using Ploeh.AutoFixture;
    using Services;

    public class TestCustomizations : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<ServicesController>(c => c
                .OmitAutoProperties());

            fixture.Customize<RequestsController>(c => c
                .OmitAutoProperties());

            fixture.Customize<TokensController>(c => c
                .OmitAutoProperties());

            fixture.Customize<IJurisdictionService>(c => c
                .FromFactory(() =>
                {
                    var storeFactory = new Mock<IJurisdictionService>();
                    storeFactory
                        .Setup(sf => sf.GetServiceStore(It.IsRegex(@"^(?!invalid)")))
                        .Returns(Task.FromResult(fixture.Create<IServiceStore>()));

                    storeFactory
                        .Setup(sf => sf.GetServiceRequestSearchService(It.IsRegex(@"^(?!invalid)")))
                        .Returns(Task.FromResult(fixture.Create<IServiceRequestSearchService>()));

                    return storeFactory.Object;
                })
            );

            fixture.Customize<IServiceAttributeValidator>(c => c
                .FromFactory(() =>
                {
                    var validator = new Mock<IServiceAttributeValidator>();
                    validator
                        .Setup(v => v.ValidateMetadata(It.IsAny<Service>(), It.IsAny<PostServiceRequestInputModel>()))
                        .Returns(Task.FromResult(new List<ValidationResult>()));

                    return validator.Object;
                })
            );

            fixture.Customize<JsonSerializerSettings>(c => c
                .FromFactory(() =>
                {
                    var jsonOptions = new MvcJsonOptions();
                    new Startup().SetupJsonOptions(jsonOptions);

                    return jsonOptions.SerializerSettings;
                })
                .OmitAutoProperties()
            );

            fixture.Customize<SnakeCaseNamingStrategy>(c => c
                .FromFactory(() =>
                {
                    var jsonOptions = new MvcJsonOptions();
                    new Startup().SetupJsonOptions(jsonOptions);

                    var contractResolver = (DefaultContractResolver) jsonOptions.SerializerSettings.ContractResolver;

                    return (SnakeCaseNamingStrategy) contractResolver.NamingStrategy;
                })
                .OmitAutoProperties()
            );
        }
    }
}
