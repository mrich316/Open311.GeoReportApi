namespace Open311.GeoReportApi.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using GeoReportApi.Controllers;
    using InputModels;
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
            Assert.Throws<ArgumentNullException>("serviceStore", () => new ServicesController(null));
        }

        public class GetServiceList
        {
            [Theory, TestConventions]
            public async Task JurisdictionId_WithoutServicesReturnsNotFound([Frozen] IServiceStore serviceStore, ServicesController sut, GetServiceListInputModel model)
            {
                var mockStore = Mock.Get(serviceStore);
                mockStore
                    .Setup(s => s.GetServices(It.IsAny<string>()))
                    .Returns(Task.FromResult(Enumerable.Empty<Service>()));

                var result = await sut.GetServiceList(model);

                Assert.IsType<NotFoundObjectResult>(result);

                var actual = ((NotFoundObjectResult) result).Value;
                Assert.IsType<Error>(actual);
                Assert.Equal(404, ((Error) actual).Code);
            }

            [Theory, TestConventions]
            public async Task JurisdictionId_WithServicesReturnsOk([Frozen] IServiceStore serviceStore, ServicesController sut, GetServiceListInputModel model, Service service)
            {
                var mockStore = Mock.Get(serviceStore);
                mockStore
                    .Setup(s => s.GetServices(It.IsAny<string>()))
                    .Returns(Task.FromResult<IEnumerable<Service>>(new[] {service}));

                var result = await sut.GetServiceList(model);

                Assert.IsType<OkObjectResult>(result);

                var actual = ((OkObjectResult)result).Value;
            }
        }
    }
}
