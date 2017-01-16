namespace Open311.GeoReportApi.Tests
{
    using System.Threading.Tasks;
    using GeoReportApi.Controllers;
    using Moq;
    using Ploeh.AutoFixture;
    using Services;
    using Services.TestStores;

    public class TestCustomizations : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<ServicesController>(c => c
                .OmitAutoProperties());

            fixture.Customize<IJurisdictionService>(c => c
                .FromFactory(() =>
                {
                    var storeFactory = new Mock<IJurisdictionService>();
                    storeFactory
                        .Setup(sf => sf.GetServiceStore(It.IsRegex(@"^(?!invalid)")))
                        .Returns(Task.FromResult(fixture.Create<IServiceStore>()));

                    return storeFactory.Object;
                })
            );
        }
    }
}
