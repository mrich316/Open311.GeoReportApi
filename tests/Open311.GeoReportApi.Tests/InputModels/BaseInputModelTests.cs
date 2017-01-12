namespace Open311.GeoReportApi.Tests.InputModels
{
    using GeoReportApi.InputModels;
    using Xunit;

    public class BaseInputModelTests
    {
        [Fact]
        public void JurisdictionIdDefaultsToOpen311Options()
        {
            var sut = new BaseInputModel();

            Assert.Equal(Open311Options.DefaultJurisdictionId, sut.JurisdictionId);
        }
    }
}
