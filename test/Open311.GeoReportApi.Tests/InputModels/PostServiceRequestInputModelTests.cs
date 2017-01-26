namespace Open311.GeoReportApi.Tests.InputModels
{
    using GeoReportApi.InputModels;
    using Xunit;

    public class PostServiceRequestInputModelTests
    {
        public class Validate
        {
            [Theory, TestConventions]
            public void MissingLatReturnsValidationError(PostServiceRequestInputModel sut)
            {
                sut.Lat = null;

                var actual = sut.Validate(null);

                Assert.Contains(actual, x => x.ErrorMessage.Contains("latitude"));
            }

            [Theory, TestConventions]
            public void MissingLongReturnsValidationError(PostServiceRequestInputModel sut)
            {
                sut.Long = null;

                var actual = sut.Validate(null);

                Assert.Contains(actual, x => x.ErrorMessage.Contains("longitude"));
            }

            [Theory, TestConventions]
            public void MissingLocationParametersReturnsValidationError(PostServiceRequestInputModel sut)
            {
                sut.Lat = null;
                sut.Long = null;
                sut.AddressId = null;
                sut.AddressString = null;

                var actual = sut.Validate(null);

                Assert.Contains(actual, x => x.ErrorMessage.Contains("location"));
            }
        }
    }
}
