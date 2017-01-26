namespace Open311.GeoReportApi.Tests.InputModels
{
    using System;
    using GeoReportApi.InputModels;
    using Xunit;

    public class GetServiceRequestsInputModelTests
    {
        public class Validate
        {
            [Theory, TestConventions]
            public void StartDateIsRequiredIfEndDateIsSpecified(GetServiceRequestsInputModel sut)
            {
                sut.StartDate = null;

                Assert.Contains(sut.Validate(null), x => x.ErrorMessage.Contains("start date"));
            }

            [Theory, TestConventions]
            public void StartDateMustBeLowerThanEndDate(GetServiceRequestsInputModel sut)
            {
                sut.StartDate = DateTimeOffset.Now;
                sut.EndDate = DateTimeOffset.Now - TimeSpan.FromDays(1);

                Assert.Contains(sut.Validate(null), x => x.ErrorMessage.Contains("date"));
            }
        }
    }
}
