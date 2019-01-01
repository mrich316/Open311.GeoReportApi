using AutoFixture.Xunit2;

namespace Open311.GeoReportApi.Tests.Filters
{
    using System;
    using System.Threading.Tasks;
    using GeoReportApi.Filters;
    using GeoReportApi.InputModels;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Models;
    using Moq;
    using Services;
    using Xunit;

    public class ValidateJurisdictionAttributeTests
    {
        [Fact]
        public void CtorThrowsOnNullJurisdiction()
        {
            Assert.Throws<ArgumentNullException>("jurisdiction", () => new ValidateJurisdictionAttribute(null));
        }

        public class OnActionExecutionAsync
        {
            [Theory, TestConventions]
            public async Task ReturnsOnResultSet(
                ValidateJurisdictionAttribute sut,
                ActionExecutingContext context,
                ActionExecutionDelegate next)
            {
                Assert.NotNull(context.Result);

                var expected = context.Result;

                await sut.OnActionExecutionAsync(context, next);

                Assert.Same(expected, context.Result);
            }

            [Theory, TestConventions]
            public async Task ReturnsOnExistingJurisdiction(
                [Frozen] IJurisdictionService jurisdiction,
                ValidateJurisdictionAttribute sut,
                BaseInputModel model,
                ActionExecutingContext context,
                ActionExecutionDelegate next)
            {
                context.ActionArguments.Clear();
                context.ActionArguments.Add("model", model);

                var mock = Mock.Get(jurisdiction);
                mock.Setup(p => p.Exists(model.JurisdictionId))
                    .Returns(Task.FromResult(true));

                context.Result = null;

                await sut.OnActionExecutionAsync(context, next);
                mock.Verify();

                Assert.Null(context.Result);
            }

            [Theory, TestConventions]
            public async Task ReturnsErrorOnInvalidJurisdiction(
                [Frozen] IJurisdictionService jurisdiction,
                ValidateJurisdictionAttribute sut,
                BaseInputModel model,
                ActionExecutingContext context,
                ActionExecutionDelegate next)
            {
                context.ActionArguments.Clear();
                context.ActionArguments.Add("model", model);

                var mock = Mock.Get(jurisdiction);
                mock.Setup(p => p.Exists(model.JurisdictionId))
                    .Returns(Task.FromResult(false));

                context.Result = null;

                await sut.OnActionExecutionAsync(context, next);
                mock.Verify();

                Assert.IsType<NotFoundObjectResult>(context.Result);

                var result = (NotFoundObjectResult) context.Result;
                Assert.Contains((Errors) result.Value, e => e.Code == 404);
            }
        }
    }
}
