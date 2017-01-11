namespace Open311.GeoReportApi.Tests
{
    using GeoReportApi.Controllers;
    using Ploeh.AutoFixture;

    public class TestCustomizations : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<ServicesController>(c => c
                .OmitAutoProperties());
        }
    }
}
