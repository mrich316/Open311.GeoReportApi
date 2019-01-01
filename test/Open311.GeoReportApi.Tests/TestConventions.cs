using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace Open311.GeoReportApi.Tests
{
    public class TestConventions : AutoDataAttribute
    {
        public TestConventions()
            : base(() => new Fixture()
                  .Customize(new TestCustomizations())
                  .Customize(new AutoMoqCustomization()))
        {
            // empty            
        }
    }
}
