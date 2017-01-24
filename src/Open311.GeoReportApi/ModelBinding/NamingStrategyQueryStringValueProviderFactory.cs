namespace Open311.GeoReportApi.ModelBinding
{
    using System;
    using System.Threading.Tasks;
    using System.Globalization;
    using Microsoft.AspNetCore.Mvc.Internal;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Newtonsoft.Json.Serialization;

    public class NamingStrategyQueryStringValueProviderFactory : IValueProviderFactory
    {
        private readonly NamingStrategy _namingStrategy;

        public NamingStrategyQueryStringValueProviderFactory(NamingStrategy namingStrategy)
        {
            if (namingStrategy == null) throw new ArgumentNullException(nameof(namingStrategy));
            _namingStrategy = namingStrategy;
        }

        public Task CreateValueProviderAsync(ValueProviderFactoryContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var valueProvider = new NamingStrategyQueryStringValueProvider(
                _namingStrategy,
                BindingSource.Query,
                context.ActionContext.HttpContext.Request.Query,
                CultureInfo.InvariantCulture);

            context.ValueProviders.Add(valueProvider);

            return TaskCache.CompletedTask;
        }
    }
}
