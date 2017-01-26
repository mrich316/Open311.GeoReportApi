namespace Open311.GeoReportApi.Tests.ModelBinding
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using GeoReportApi.ModelBinding;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Models;
    using Testing;
    using Xunit;

    public class CommaDelimitedListModelBinderTests
    {
        public class BindModelAsync
        {
            [Theory, TestConventions]
            public async Task SingleStringReturnsListOfT(CommaDelimitedListModelBinder sut, string value)
            {
                var bindingContext = GetBindingContext(typeof(List<string>));
                bindingContext.ValueProvider = new SimpleValueProvider
                {
                    {"theModelName", value}
                };

                await sut.BindModelAsync(bindingContext);

                var actual = bindingContext.Result.Model;
                var expected = new List<string> {value};

                Assert.Equal(expected, actual);
            }

            [Theory, TestConventions]
            public async Task MultipleStringsReturnsListOfT(CommaDelimitedListModelBinder sut, string value1, string value2)
            {
                var bindingContext = GetBindingContext(typeof(List<string>));
                bindingContext.ValueProvider = new SimpleValueProvider
                {
                    {"theModelName", string.Join(",", value1, value2)}
                };

                await sut.BindModelAsync(bindingContext);

                var actual = bindingContext.Result.Model;
                var expected = new List<string> { value1, value2 };

                Assert.Equal(expected, actual);
            }

            [Theory, TestConventions]
            public async Task MultipleStringsAndEmptyReturnsListOfT(CommaDelimitedListModelBinder sut, string value1, string value2)
            {
                var bindingContext = GetBindingContext(typeof(List<string>));
                bindingContext.ValueProvider = new SimpleValueProvider
                {
                    {"theModelName", string.Join(",, ", value1, value2)}
                };

                await sut.BindModelAsync(bindingContext);

                var actual = bindingContext.Result.Model;
                var expected = new List<string> { value1, value2 };

                Assert.Equal(expected, actual);
            }

            [Theory, TestConventions]
            public async Task NullStringReturnsListOfT(CommaDelimitedListModelBinder sut)
            {
                var bindingContext = GetBindingContext(typeof(List<string>));
                bindingContext.ValueProvider = new SimpleValueProvider
                {
                    {"theModelName", null}
                };

                await sut.BindModelAsync(bindingContext);

                var actual = bindingContext.Result.Model;
                var expected = new List<string>();

                Assert.Equal(expected, actual);
            }

            [Theory, TestConventions]
            public async Task SingleServiceRequestStatusReturnsListOfT(CommaDelimitedListModelBinder sut)
            {
                var bindingContext = GetBindingContext(typeof(List<ServiceRequestStatus>));
                bindingContext.ValueProvider = new SimpleValueProvider
                {
                    {"theModelName", ServiceRequestStatus.Open.ToString()}
                };

                await sut.BindModelAsync(bindingContext);

                var actual = bindingContext.Result.Model;
                var expected = new List<ServiceRequestStatus> { ServiceRequestStatus.Open };

                Assert.Equal(expected, actual);
            }

            [Theory, TestConventions]
            public async Task MultipleServiceRequestStatusReturnsListOfT(CommaDelimitedListModelBinder sut)
            {
                var bindingContext = GetBindingContext(typeof(List<ServiceRequestStatus>));
                bindingContext.ValueProvider = new SimpleValueProvider
                {
                    {"theModelName", string.Join(",", ServiceRequestStatus.Open, ServiceRequestStatus.Closed)}
                };

                await sut.BindModelAsync(bindingContext);

                var actual = bindingContext.Result.Model;
                var expected = new List<ServiceRequestStatus> { ServiceRequestStatus.Open, ServiceRequestStatus.Closed };

                Assert.Equal(expected, actual);
            }

            [Theory, TestConventions]
            public async Task InvalidServiceRequestStatusReturnsListOfT(CommaDelimitedListModelBinder sut)
            {
                var bindingContext = GetBindingContext(typeof(List<ServiceRequestStatus>));
                bindingContext.ValueProvider = new SimpleValueProvider
                {
                    {"theModelName", "invalid"}
                };

                await sut.BindModelAsync(bindingContext);

                Assert.Null(bindingContext.Result.Model);
                Assert.False(bindingContext.ModelState.IsValid);
            }
        }

        private static DefaultModelBindingContext GetBindingContext(Type modelType)
        {
            return new DefaultModelBindingContext
            {
                ModelMetadata = new EmptyModelMetadataProvider().GetMetadataForType(modelType),
                ModelName = "theModelName",
                ModelState = new ModelStateDictionary(),
                ValueProvider = new SimpleValueProvider() // empty
            };
        }
    }
}