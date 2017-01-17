namespace Open311.GeoReportApi.ModelBinding
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Newtonsoft.Json.Serialization;

    public class NamingStrategyFormValueProvider : FormValueProvider
    {
        private readonly NamingStrategy _namingStrategy;
        private readonly IFormCollection _values;
        private readonly CultureInfo _culture;

        public NamingStrategyFormValueProvider(
            NamingStrategy namingStrategy, BindingSource bindingSource,
            IFormCollection values,
            CultureInfo culture)
            : base(bindingSource, values, culture)
        {
            if (namingStrategy == null) throw new ArgumentNullException(nameof(namingStrategy));

            _namingStrategy = namingStrategy;
            _values = values;
            _culture = culture;
        }

        public override ValueProviderResult GetValue(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            key = string.Join(".", key.Split('.')
                .Select(k => _namingStrategy.GetPropertyName(k, false)));

            ValueProviderResult result;

            // TODO: What should happen if values are provided with and without php style arrays ?
            // Example: If we received: attribute[code][]=1&attribute[code]=2
            // Should we return attribute=1,2 ? In this implementation we only
            // return attribute=2.

            var values = _values[key];
            if (values.Count == 0)
            {
                // georeport api supports php style arrays. check if present.
                values = _values[key + "[]"];

                result = values.Count == 0
                    ? ValueProviderResult.None
                    : new ValueProviderResult(values, _culture);
            }
            else
            {
                result = new ValueProviderResult(values, _culture);
            }

            return result;
        }
    }
}
