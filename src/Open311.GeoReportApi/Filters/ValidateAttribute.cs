namespace Open311.GeoReportApi.Filters
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Models;

    public class ValidateAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Transform model state errors to a 400-BadRequest using GeoReports errors. 
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid) return;

            var errors = new Errors();

            foreach (var property in context.ModelState
                .Where(kvp => kvp.Value.ValidationState == ModelValidationState.Invalid))
            {
                foreach (var error in property.Value.Errors)
                {
                    errors.Add(400, error.ErrorMessage ?? error.Exception.Message);
                }
            }

            context.Result = new BadRequestObjectResult(errors);
        }

        /// <summary>
        /// Ensure api compliance by encapsuling a single <see cref="Error"/>
        /// in <see cref="Errors"/>.
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var result = context.Result as ObjectResult;
            var value = result?.Value as Error;

            if (value != null)
            {
                result.Value = new Errors(value);
            }
        }
    }
}
