namespace Open311.GeoReportApi.Tests.Testing
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    // copied from https://github.com/aspnet/Mvc/blob/dev/test/Microsoft.AspNetCore.Mvc.TestCommon/SimpleValueProvider.cs
    public class SimpleValueProvider : Dictionary<string, object>, IValueProvider
    {
        private readonly CultureInfo _culture;

        public SimpleValueProvider()
            : this(null)
        {
        }

        public SimpleValueProvider(CultureInfo culture)
            : base(StringComparer.OrdinalIgnoreCase)
        {
            _culture = culture ?? CultureInfo.InvariantCulture;
        }

        public bool ContainsPrefix(string prefix)
        {
            foreach (string key in Keys)
            {
                if (ModelStateDictionary.StartsWithPrefix(prefix, key))
                {
                    return true;
                }
            }

            return false;
        }

        public ValueProviderResult GetValue(string key)
        {
            object rawValue;
            if (TryGetValue(key, out rawValue))
            {
                if (rawValue != null && rawValue.GetType().IsArray)
                {
                    var array = (Array)rawValue;

                    var stringValues = new string[array.Length];
                    for (var i = 0; i < array.Length; i++)
                    {
                        stringValues[i] = array.GetValue(i) as string ?? Convert.ToString(array.GetValue(i), _culture);
                    }

                    return new ValueProviderResult(stringValues, _culture);
                }
                else
                {
                    var stringValue = rawValue as string ?? Convert.ToString(rawValue, _culture) ?? string.Empty;
                    return new ValueProviderResult(stringValue, _culture);
                }
            }

            return ValueProviderResult.None;
        }
    }
}
