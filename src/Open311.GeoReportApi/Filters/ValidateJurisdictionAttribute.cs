namespace Open311.GeoReportApi.Filters
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using InputModels;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Models;
    using Services;

    public class ValidateJurisdictionAttribute : ActionFilterAttribute
    {
        private readonly IServiceStoreFactory _serviceStoreFactory;

        public ValidateJurisdictionAttribute(IServiceStoreFactory serviceStoreFactory)
        {
            if (serviceStoreFactory == null) throw new ArgumentNullException(nameof(serviceStoreFactory));
            _serviceStoreFactory = serviceStoreFactory;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.Result != null) return;

            foreach (var model in context.ActionArguments.Values.OfType<BaseInputModel>())
            {
                if (!await _serviceStoreFactory.JurisdictionExists(model.JurisdictionId))
                {
                    context.Result = new NotFoundObjectResult(new Errors(new Error
                    {
                        Code = 404,
                        Description = $"{Open311Constants.ModelProperties.JurisdictionId} was not found."
                    }));

                    break;
                }
            }
        }
    }
}
