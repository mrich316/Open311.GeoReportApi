namespace Open311.GeoReportApi.ModelBinding
{
    using System;
    using System.Globalization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Newtonsoft.Json.Serialization;

    public class NamingStrategyQueryStringValueProvider : QueryStringValueProvider
    {
        private readonly NamingStrategy _namingStrategy;

        public NamingStrategyQueryStringValueProvider(
            NamingStrategy namingStrategy, BindingSource bindingSource, IQueryCollection values, CultureInfo culture)
            : base(bindingSource, values, culture)
        {
            if (namingStrategy == null) throw new ArgumentNullException(nameof(namingStrategy));
            _namingStrategy = namingStrategy;
        }

        public override ValueProviderResult GetValue(string key)
        {
            key = _namingStrategy.GetPropertyName(key, false);
            return base.GetValue(key);
        }
    }
}
