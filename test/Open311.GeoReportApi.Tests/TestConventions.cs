namespace Open311.GeoReportApi.Tests
{
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;
    using Ploeh.AutoFixture.Xunit2;

    public class TestConventions : AutoDataAttribute
    {
        public TestConventions()
            : base(new Fixture()
                  .Customize(new TestCustomizations())
                  .Customize(new AutoMoqCustomization()))
        {
            // empty            
        }
    }
}
