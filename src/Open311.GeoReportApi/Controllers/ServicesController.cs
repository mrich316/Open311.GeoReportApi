namespace Open311.GeoReportApi.Controllers
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using InputModels;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Services;

    [FormatFilter]
    [Route(Open311Constants.Routes.GeoReportV2)]
    public class ServicesController : Controller
    {
        private readonly IServiceStoreFactory _storeFactory;

        public ServicesController(IServiceStoreFactory storeFactory)
        {
            if (storeFactory == null) throw new ArgumentNullException(nameof(storeFactory));
            _storeFactory = storeFactory;
        }

        [HttpGet("services.{format}")]
        public async Task<IActionResult> GetServiceList(GetServiceListInputModel model, CancellationToken cancellationToken)
        {
            var store = await GetServiceStore(model);
            var services = await store
                .GetServices(cancellationToken);

            var serviceList = new Services<Service>(services);

            var result = serviceList.Any()
                ? Ok(serviceList)
                : NotFound(404, $"{Open311Constants.ModelProperties.JurisdictionId} provided was not found");

            return result;
        }

        [HttpGet("services/{service_code}.{format}")]
        public async Task<IActionResult> GetServiceDefinition(GetServiceDefinitionInputModel model, CancellationToken cancellationToken)
        {
            var store = await GetServiceStore(model);
            var definition = await store
                .GetServiceDefinition(model.ServiceCode, cancellationToken);

            var result = definition != null
                ? Ok(definition)
                : NotFound(404,
                    $"{Open311Constants.ModelProperties.ServiceCode} or {Open311Constants.ModelProperties.JurisdictionId} provided was not found.");

            return result;
        }

        [HttpGet("services/requests.{format}")]
        public IActionResult PostServiceRequest(PostServiceRequestInputModel model)
        {
            // TODO: Requires API key.
            throw new NotImplementedException();
        }

        protected virtual Task<IServiceStore> GetServiceStore<TModel>(TModel model) where TModel : BaseInputModel
        {
            return _storeFactory.GetServiceStore(model.JurisdictionId);
        }

        protected virtual IActionResult NotFound(int code, string description)
        {
            return NotFound(new Error
            {
                Code = code,
                Description = description
            });
        }
    }
}
