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
        private readonly IJurisdictionService _serviceStoreFactory;

        public ValidateJurisdictionAttribute(IJurisdictionService jurisdiction)
        {
            if (jurisdiction == null) throw new ArgumentNullException(nameof(jurisdiction));
            _serviceStoreFactory = jurisdiction;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.Result == null)
            {
                foreach (var model in context.ActionArguments.Values.OfType<BaseInputModel>())
                {
                    if (!await _serviceStoreFactory.Exists(model.JurisdictionId))
                    {
                        context.Result = new NotFoundObjectResult(new Errors(new Error
                        {
                            Code = 404,
                            Description = $"{Open311Constants.ModelProperties.JurisdictionId} provided was not found."
                        }));

                        break;
                    }
                }
            }

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
