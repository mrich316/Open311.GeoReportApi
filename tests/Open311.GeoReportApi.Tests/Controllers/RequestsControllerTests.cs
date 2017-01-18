namespace Open311.GeoReportApi.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
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

    public class RequestsControllerTests
    {
        [Fact]
        public void Ctor_ThrowsOnArgumentNull()
        {
            Assert.Throws<ArgumentNullException>("jurisdiction", () => new ServicesController(null));
        }

        public class GetServiceRequest
        {
            [Theory, TestConventions]
            public async Task WithoutServiceRequestReturnsNotFound(
                [Frozen] IServiceRequestSearchService searchService,
                RequestsController sut,
                GetServiceRequestInputModel model)
            {
                var mockService = Mock.Get(searchService);
                mockService
                    .Setup(s => s.Get(It.IsAny<IEnumerable<string>>(), CancellationToken.None))
                    .Returns(Task.FromResult(Enumerable.Empty<ServiceRequest>()));

                var result = await sut.GetServiceRequest(model, CancellationToken.None);
                mockService.Verify();

                Assert.IsType<NotFoundObjectResult>(result);

                var actual = ((NotFoundObjectResult)result).Value;
                Assert.IsType<Error>(actual);
                Assert.Equal(404, ((Error)actual).Code);
            }
            
            [Theory, TestConventions]
            public async Task WithServicesReturnsOk(
                [Frozen] IServiceRequestSearchService searchService,
                RequestsController sut,
                GetServiceRequestInputModel model, ServiceRequest expected)
            {
                var mockService = Mock.Get(searchService);
                mockService
                    .Setup(s => s.Get(It.IsAny<IEnumerable<string>>(), CancellationToken.None))
                    .Returns(Task.FromResult<IEnumerable<ServiceRequest>>(new[] { expected }));

                var result = await sut.GetServiceRequest(model, CancellationToken.None);
                mockService.Verify();

                Assert.IsType<OkObjectResult>(result);

                var actual = ((OkObjectResult)result).Value;
                Assert.IsType<ServiceRequest>(actual);
                Assert.Equal(expected, (ServiceRequest) actual);
            }
        }

        public class GetServiceRequests
        {
            [Theory, TestConventions]
            public async Task UsingNonExistentServiceRequestIdsReturnsOk(
                [Frozen] IServiceRequestSearchService searchService,
                RequestsController sut,
                GetServiceRequestsInputModel model)
            {
                var mockService = Mock.Get(searchService);
                mockService
                    .Setup(s => s.Get(It.IsAny<IEnumerable<string>>(), CancellationToken.None))
                    .Returns(Task.FromResult(Enumerable.Empty<ServiceRequest>()));

                var result = await sut.GetServiceRequests(model, CancellationToken.None);
                mockService.Verify();
                mockService.Verify(s => s.Search(It.IsAny<ServiceRequestQuery>(), CancellationToken.None), Times.Never());

                Assert.IsType<OkObjectResult>(result);

                var actual = ((OkObjectResult)result).Value;
                Assert.IsType<ServiceRequests<ServiceRequest>>(actual);
                Assert.Equal(0, ((ServiceRequests<ServiceRequest>)actual).Count);
            }

            [Theory, TestConventions]
            public async Task UsingExistingServiceRequestIdsReturnsOk(
                [Frozen] IServiceRequestSearchService searchService,
                RequestsController sut,
                GetServiceRequestsInputModel model, List<ServiceRequest> expected)
            {
                var mockService = Mock.Get(searchService);
                mockService
                    .Setup(s => s.Get(It.IsAny<IEnumerable<string>>(), CancellationToken.None))
                    .Returns(Task.FromResult<IEnumerable<ServiceRequest>>(expected));

                var result = await sut.GetServiceRequests(model, CancellationToken.None);
                mockService.Verify();
                mockService.Verify(s => s.Search(It.IsAny<ServiceRequestQuery>(), CancellationToken.None), Times.Never());

                Assert.IsType<OkObjectResult>(result);

                var actual = ((OkObjectResult)result).Value;
                Assert.IsType<ServiceRequests<ServiceRequest>>(actual);
                Assert.Equal(expected, (ServiceRequests<ServiceRequest>)actual);
            }

            [Theory, TestConventions]
            public async Task UsingOtherPropertiesReturnsOk(
                [Frozen] IServiceRequestSearchService searchService,
                RequestsController sut,
                GetServiceRequestsInputModel model, List<ServiceRequest> expected)
            {
                model.ServiceRequestId.Clear();

                var mockService = Mock.Get(searchService);
                mockService
                    .Setup(s => s.Search(It.IsAny<ServiceRequestQuery>(), CancellationToken.None))
                    .Returns(Task.FromResult<IEnumerable<ServiceRequest>>(expected));

                var result = await sut.GetServiceRequests(model, CancellationToken.None);
                mockService.Verify();
                mockService.Verify(s => s.Get(It.IsAny<IEnumerable<string>>(), CancellationToken.None), Times.Never());

                Assert.IsType<OkObjectResult>(result);

                var actual = ((OkObjectResult)result).Value;
                Assert.IsType<ServiceRequests<ServiceRequest>>(actual);
                Assert.Equal(expected, (ServiceRequests<ServiceRequest>)actual);
            }
        }

        public class PostServiceRequest
        {
            [Theory, TestConventions]
            public async Task NonExistentServiceCodeReturnsNotFound(
                [Frozen] IServiceStore store,
                RequestsController sut,
                PostServiceRequestInputModel model)
            {
                var mockStore = Mock.Get(store);
                mockStore
                    .Setup(s => s.GetService(model.ServiceCode, CancellationToken.None))
                    .Returns(Task.FromResult<Service>(null));

                var result = await sut.PostServiceRequest(model);
                mockStore.Verify();

                Assert.IsType<NotFoundObjectResult>(result);

                var actual = ((NotFoundObjectResult)result).Value;
                Assert.IsType<Error>(actual);
                Assert.Equal(404, ((Error)actual).Code);
            }

            [Theory, TestConventions]
            public async Task AttributeValidationErrorsReturnsBadRequest(
                [Frozen] IServiceStore store,
                [Frozen] IServiceAttributeValidator validator,
                RequestsController sut,
                PostServiceRequestInputModel model,
                Service service)
            {
                var mockStore = Mock.Get(store);
                mockStore
                    .Setup(s => s.GetService(model.ServiceCode, CancellationToken.None))
                    .Returns(Task.FromResult(service));

                var mockValidator = Mock.Get(validator);
                mockValidator
                    .Setup(s => s.ValidateMetadata(service, model))
                    .Returns(Task.FromResult(new List<ValidationResult>
                    {
                        new ValidationResult("oups")
                    }));

                var result = await sut.PostServiceRequest(model);
                mockStore.Verify();
                mockValidator.Verify();

                Assert.IsType<BadRequestObjectResult>(result);

                var actual = ((BadRequestObjectResult)result).Value;
                Assert.IsType<Errors>(actual);
                Assert.Equal(400, ((Errors)actual).FirstOrDefault()?.Code);
            }

            [Theory, TestConventions]
            public async Task ValidPostReturnsOk(
                [Frozen] IServiceStore store,
                RequestsController sut,
                PostServiceRequestInputModel model,
                Service service,
                ServiceRequestCreated created)
            {
                var mockStore = Mock.Get(store);
                mockStore
                    .Setup(s => s.GetService(model.ServiceCode, CancellationToken.None))
                    .Returns(Task.FromResult(service));

                mockStore
                    .Setup(s => s.Create(model))
                    .Returns(Task.FromResult(created));

                var result = await sut.PostServiceRequest(model);
                mockStore.Verify();

                Assert.IsType<OkObjectResult>(result);

                var actual = ((OkObjectResult)result).Value;
                Assert.IsType<ServiceRequests<ServiceRequestCreated>>(actual);
                Assert.Equal(created, ((ServiceRequests<ServiceRequestCreated>)actual).FirstOrDefault());
            }
        }
    }
}
