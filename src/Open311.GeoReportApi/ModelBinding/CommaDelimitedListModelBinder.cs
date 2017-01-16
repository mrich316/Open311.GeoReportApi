namespace Open311.GeoReportApi.ModelBinding
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class CommaDelimitedListModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (!bindingContext.ModelType.IsGenericType
                || typeof(List<>) != bindingContext.ModelType.GetGenericTypeDefinition())
            {
                return Task.CompletedTask;
            }

            var elementType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];
            var list = (IList)Activator.CreateInstance(bindingContext.ModelType);

            var key = bindingContext.ModelName;
            var value = bindingContext.ValueProvider.GetValue(key).ToString();

            if (!string.IsNullOrWhiteSpace(value))
            {
                var converter = TypeDescriptor.GetConverter(elementType);

                foreach (var elm in value.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries))
                {
                    try
                    {
                        list.Add(converter.ConvertFromString(elm.Trim()));
                    }
                    catch
                    {
                        // TODO: What should we do when an element is invalid ?
                        // This implementation simply removes invalid elements.
                    }
                }
            }

            bindingContext.Result = ModelBindingResult.Success(list);

            return Task.CompletedTask;
        }
    }
}