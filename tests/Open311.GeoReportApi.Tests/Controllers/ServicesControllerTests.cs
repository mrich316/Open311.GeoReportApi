namespace Open311.GeoReportApi.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using GeoReportApi.Controllers;
    using GeoReportApi.InputModels;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Moq;
    using Ploeh.AutoFixture.Xunit2;
    using Services;
    using Xunit;

    public class ServicesControllerTests
    {
        [Fact]
        public void Ctor_ThrowsOnArgumentNull()
        {
            Assert.Throws<ArgumentNullException>("storeFactory", () => new ServicesController(null));
        }

        public class GetServiceList
        {
            [Theory, TestConventions]
            public async Task JurisdictionIdWithoutServicesReturnsNotFound([Frozen] IServiceStore serviceStore, ServicesController sut, GetServiceListInputModel model)
            {
                var mockStore = Mock.Get(serviceStore);
                mockStore
                    .Setup(s => s.GetServices(CancellationToken.None))
                    .Returns(Task.FromResult(Enumerable.Empty<Service>()));

                var result = await sut.GetServiceList(model, CancellationToken.None);

                Assert.IsType<NotFoundObjectResult>(result);

                var actual = ((NotFoundObjectResult) result).Value;
                Assert.IsType<Error>(actual);
                Assert.Equal(404, ((Error) actual).Code);
            }

            [Theory, TestConventions]
            public async Task JurisdictionIdWithServicesReturnsOk([Frozen] IServiceStore serviceStore, ServicesController sut, GetServiceListInputModel model, Service expected)
            {
                var mockStore = Mock.Get(serviceStore);
                mockStore
                    .Setup(s => s.GetServices(CancellationToken.None))
                    .Returns(Task.FromResult<IEnumerable<Service>>(new[] {expected}));

                var result = await sut.GetServiceList(model, CancellationToken.None);

                Assert.IsType<OkObjectResult>(result);

                var actual = ((OkObjectResult)result).Value;
                Assert.IsType<Services<Service>>(actual);
                Assert.Equal(expected, ((Services<Service>)actual).FirstOrDefault());
            }
        }
    }
}
