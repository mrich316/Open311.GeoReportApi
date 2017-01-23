namespace Open311.GeoReportApi.ModelBinding
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Internal;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// A <see cref="IValueProviderFactory"/> for <see cref="NamingStrategyFormValueProvider"/>
    /// </summary>
    /// <remarks>
    /// adapted from https://github.com/aspnet/Mvc/blob/master/src/Microsoft.AspNetCore.Mvc.Core/ModelBinding/FormValueProvider.cs
    /// </remarks>
    public class NamingStrategyFormValueProviderFactory : IValueProviderFactory
    {
        private readonly NamingStrategy _namingStrategy;

        public NamingStrategyFormValueProviderFactory(NamingStrategy namingStrategy)
        {
            if (namingStrategy == null) throw new ArgumentNullException(nameof(namingStrategy));
            _namingStrategy = namingStrategy;
        }

        public Task CreateValueProviderAsync(ValueProviderFactoryContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var request = context.ActionContext.HttpContext.Request;
            if (request.HasFormContentType)
            {
                // Allocating a Task only when the body is form data.
                return AddValueProviderAsync(context);
            }

            return TaskCache.CompletedTask;
        }

        private async Task AddValueProviderAsync(ValueProviderFactoryContext context)
        {
            var request = context.ActionContext.HttpContext.Request;
            var valueProvider = new NamingStrategyFormValueProvider(
                _namingStrategy,
                BindingSource.Form,
                await request.ReadFormAsync(),
                CultureInfo.CurrentCulture);

            context.ValueProviders.Add(valueProvider);
        }
    }
}
