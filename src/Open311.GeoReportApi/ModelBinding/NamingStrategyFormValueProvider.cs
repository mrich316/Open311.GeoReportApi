namespace Open311.GeoReportApi.ModelBinding
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.Extensions.Primitives;
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

        public override bool ContainsPrefix(string prefix)
        {
            var snakeCasePrefix = _namingStrategy.GetPropertyName(prefix, false);

            return base.ContainsPrefix(snakeCasePrefix);
        }

        public override IDictionary<string, string> GetKeysFromPrefix(string prefix)
        {
            var snakeCasePrefix = _namingStrategy.GetPropertyName(prefix, false);

            return base.GetKeysFromPrefix(snakeCasePrefix);
        }

        public override ValueProviderResult GetValue(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            key = GetSnakeCaseKey(key);

            var result = base.GetValue(key);

            // georeport api supports php style arrays.
            // check if present and append them.
            var phpKey = key + "[]";

            // we use FormCollection directly because base.GetValue() uses
            // PrefixContainer and strips empty arrays in prefixes:
            // ex: attribute[code][] is seen as attribute[code].
            if (_values.ContainsKey(phpKey))
            {
                var phpValues = _values[phpKey];

                if (phpValues.Count > 0)
                {
                    var values = result.Values.Concat(phpValues);

                    result = new ValueProviderResult(new StringValues(values.ToArray()), _culture);
                }
            }

            return result;
        }

        protected string GetSnakeCaseKey(string key)
        {
            var keyParts = key.Split('.');
            var last = keyParts.Last();

            if (!last.Contains('['))
            {
                keyParts[keyParts.Length - 1] = _namingStrategy.GetPropertyName(last, false);
            }

            return string.Join(".", keyParts);
        }
    }
}
