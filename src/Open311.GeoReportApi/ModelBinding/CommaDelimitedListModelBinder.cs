namespace Open311.GeoReportApi.ModelBinding
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Reflection;
    using System.Runtime.ExceptionServices;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class CommaDelimitedListModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            var modelTypeInfo = bindingContext.ModelType.GetTypeInfo();

            try
            {
                var list = (IList) Activator.CreateInstance(bindingContext.ModelType);
                var value = valueProviderResult.FirstValue;

                if (!string.IsNullOrWhiteSpace(value))
                {
                    var elementType = modelTypeInfo.GenericTypeArguments[0];
                    var converter = TypeDescriptor.GetConverter(elementType);

                    foreach (var elm in value.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries))
                    {
                        list.Add(converter.ConvertFromString(null, valueProviderResult.Culture, elm.Trim()));
                    }
                }

                bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);
                bindingContext.Result = ModelBindingResult.Success(list);

            }
            catch (Exception exception)
            {
                // adapted from https://github.com/aspnet/Mvc/blob/dev/src/Microsoft.AspNetCore.Mvc.Core/ModelBinding/Binders/SimpleTypeModelBinder.cs

                var isFormatException = exception is FormatException;
                if (!isFormatException && exception.InnerException != null)
                {
                    // TypeConverter throws System.Exception wrapping the FormatException,
                    // so we capture the inner exception.
                    exception = ExceptionDispatchInfo.Capture(exception.InnerException).SourceException;
                }

                bindingContext.ModelState.TryAddModelError(
                    bindingContext.ModelName,
                    exception,
                    bindingContext.ModelMetadata);
            }

            return Task.CompletedTask;
        }
    }
}