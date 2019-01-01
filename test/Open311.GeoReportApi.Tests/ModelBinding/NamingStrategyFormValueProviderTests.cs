namespace Open311.GeoReportApi.Tests.ModelBinding
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using GeoReportApi.ModelBinding;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.Extensions.Primitives;
    using Newtonsoft.Json.Serialization;
    using Xunit;

    public class NamingStrategyFormValueProviderTests
    {
        public class Ctor
        {
            [Theory, TestConventions]
            public void NullNamingStrategyThrows(FormCollection values, CultureInfo culture)
            {
                Assert.Throws<ArgumentNullException>("namingStrategy",
                    () => new NamingStrategyFormValueProvider(null, BindingSource.Form, values, culture));
            }

            [Theory, TestConventions]
            public void NullBindingSourceThrows(SnakeCaseNamingStrategy namingStrategy, FormCollection values,
                CultureInfo culture)
            {
                Assert.Throws<ArgumentNullException>("bindingSource",
                    () => new NamingStrategyFormValueProvider(namingStrategy, null, values, culture));
            }

            [Theory, TestConventions]
            public void NullValuesThrows(SnakeCaseNamingStrategy namingStrategy, CultureInfo culture)
            {
                Assert.Throws<ArgumentNullException>("values",
                    () => new NamingStrategyFormValueProvider(namingStrategy, BindingSource.Form, null, culture));
            }

            [Theory, TestConventions]
            public void NullCultureReturns(SnakeCaseNamingStrategy namingStrategy, FormCollection values)
            {
                Assert.NotNull(new NamingStrategyFormValueProvider(namingStrategy, BindingSource.Form, values, null));
            }
        }

        public class ContainsPrefix
        {
            [Fact]
            public void OnAttributeReturnsTrue()
            {
                var sut = GetProvider();

                Assert.True(sut.ContainsPrefix("Attribute"));
            }

            [Fact]
            public void OnSnakeCaseReturnsTrue()
            {
                var sut = GetProvider();

                Assert.True(sut.ContainsPrefix("SnakeCase"));
            }
        }

        public class GetKeysFromPrefix
        {
            [Fact]
            public void OnAttributeReturns()
            {
                var sut = GetProvider();

                var actual = sut.GetKeysFromPrefix("Attribute");
                var expected = new Dictionary<string, string>
                {
                    {"code1", "attribute[code1]"},
                    {"code2", "attribute[code2]"},
                    {"phparrays", "attribute[phparrays]"},
                    {"PascalCase", "attribute[PascalCase]"}
                };

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void OnSnakeCaseReturns()
            {
                var sut = GetProvider();

                var actual = sut.GetKeysFromPrefix("SnakeCase");

                Assert.Empty(actual);
            }
        }

        public class GetValue
        {
            [Fact]
            public void AttributeKeysShouldNotBeSnakeCased()
            {
                var sut = GetProvider();

                var actual = sut.GetValue("attribute[PascalCase]");
                var expected = new ValueProviderResult(new StringValues(new[] { "a", "b", "c" }));

                Assert.Equal(expected, actual);
            }

            [Fact]
            public void AttributeKeysShouldBeCombinedWithPhpStyledArrays()
            {
                var sut = GetProvider();

                var actual = sut.GetValue("attribute[phparrays]");
                var expected = new ValueProviderResult(new StringValues(new[] { "a", "b", "c", "c", "d", "e" }));

                Assert.Equal(expected, actual);
            }
        }

        public static NamingStrategyFormValueProvider GetProvider()
        {
            return new NamingStrategyFormValueProvider(
                new SnakeCaseNamingStrategy(true, false), 
                BindingSource.Form,
                new FormCollection(BackingStore),
                CultureInfo.InvariantCulture);
        }

        public static readonly Dictionary<string, StringValues> BackingStore = new Dictionary<string, StringValues>
        {
            {"snake_case", StringValues.Empty},
            {"attribute[code1]", new[] {"a", "b", "c"}},
            {"attribute[code2][]", new[] {"a", "b", "c"}},
            {"attribute[phparrays]", new[] {"a", "b", "c"}},
            {"attribute[phparrays][]", new[] {"c", "d", "e"}},
            {"attribute[PascalCase]", new[] {"a", "b", "c"}}
        };
    }
}