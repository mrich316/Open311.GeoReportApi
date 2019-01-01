namespace Open311.GeoReportApi.ModelBinding
{
    using System;
    using System.Threading.Tasks;
    using System.Globalization;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Newtonsoft.Json.Serialization;

    public class NamingStrategyQueryStringValueProviderFactory : IValueProviderFactory
    {
        private readonly NamingStrategy _namingStrategy;

        public NamingStrategyQueryStringValueProviderFactory(NamingStrategy namingStrategy)
        {
            _namingStrategy = namingStrategy ?? throw new ArgumentNullException(nameof(namingStrategy));
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

            return Task.CompletedTask;
        }
    }
}
