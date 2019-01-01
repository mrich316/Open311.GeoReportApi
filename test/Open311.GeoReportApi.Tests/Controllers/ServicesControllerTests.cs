using AutoFixture.Xunit2;

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
    using Services;
    using Xunit;

    public class ServicesControllerTests
    {
        [Fact]
        public void Ctor_ThrowsOnArgumentNull()
        {
            Assert.Throws<ArgumentNullException>("jurisdiction", () => new ServicesController(null));
        }

        public class GetServiceList
        {
            [Theory, TestConventions]
            public async Task WithoutServiceReturnsNotFound(
                [Frozen] IServiceStore serviceStore,
                ServicesController sut,
                GetServiceListInputModel model)
            {
                var mockStore = Mock.Get(serviceStore);
                mockStore
                    .Setup(s => s.GetServices(CancellationToken.None))
                    .Returns(Task.FromResult(Enumerable.Empty<Service>()));

                var result = await sut.GetServiceList(model, CancellationToken.None);
                mockStore.Verify();

                Assert.IsType<NotFoundObjectResult>(result);

                var actual = ((NotFoundObjectResult) result).Value;
                Assert.IsType<Error>(actual);
                Assert.Equal(404, ((Error) actual).Code);
            }

            [Theory, TestConventions]
            public async Task WithServicesReturnsOk(
                [Frozen] IServiceStore serviceStore,
                ServicesController sut,
                GetServiceListInputModel model, Service expected)
            {
                var mockStore = Mock.Get(serviceStore);
                mockStore
                    .Setup(s => s.GetServices(CancellationToken.None))
                    .Returns(Task.FromResult<IEnumerable<Service>>(new[] {expected}));

                var result = await sut.GetServiceList(model, CancellationToken.None);
                mockStore.Verify();

                Assert.IsType<OkObjectResult>(result);

                var actual = ((OkObjectResult) result).Value;
                Assert.IsType<Services>(actual);
                Assert.Equal(expected, ((Services) actual).FirstOrDefault());
            }
        }

        public class GetServiceDefinition
        {
            [Theory, TestConventions]
            public async Task WithoutServiceDefinitionReturnsNotFound(
                [Frozen] IServiceStore serviceStore,
                ServicesController sut,
                GetServiceDefinitionInputModel model)
            {
                var mockStore = Mock.Get(serviceStore);
                mockStore
                    .Setup(s => s.GetServiceDefinition(It.IsAny<string>(), CancellationToken.None))
                    .Returns(Task.FromResult<ServiceDefinition>(null));

                var result = await sut.GetServiceDefinition(model, CancellationToken.None);
                mockStore.Verify();

                Assert.IsType<NotFoundObjectResult>(result);

                var actual = ((NotFoundObjectResult)result).Value;
                Assert.IsType<Error>(actual);
                Assert.Equal(404, ((Error)actual).Code);
            }
            
            [Theory, TestConventions]
            public async Task WithServiceDefinitionReturnsOk(
                [Frozen] IServiceStore serviceStore,
                ServicesController sut,
                GetServiceDefinitionInputModel model, ServiceDefinition expected)
            {
                var mockStore = Mock.Get(serviceStore);
                mockStore
                    .Setup(s => s.GetServiceDefinition(It.IsAny<string>(), CancellationToken.None))
                    .Returns(Task.FromResult(expected));

                var result = await sut.GetServiceDefinition(model, CancellationToken.None);
                mockStore.Verify();

                Assert.IsType<OkObjectResult>(result);

                var actual = ((OkObjectResult)result).Value;
                Assert.IsType<ServiceDefinition>(actual);
                Assert.Equal(expected, actual);
            }
        }
    }
}