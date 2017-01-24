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
    public class ServicesController : GeoReportController
    {
        private readonly IJurisdictionService _jurisdiction;

        public ServicesController(IJurisdictionService jurisdiction)
        {
            if (jurisdiction == null) throw new ArgumentNullException(nameof(jurisdiction));
            _jurisdiction = jurisdiction;
        }

        [HttpGet("services.{format}")]
        public async Task<IActionResult> GetServiceList(GetServiceListInputModel model, CancellationToken cancellationToken)
        {
            var store = await _jurisdiction.GetServiceStore(model);
            var services = await store.GetServices(cancellationToken);

            var serviceList = new Services(services);

            var result = serviceList.Any()
                ? Ok(serviceList)
                : NotFound(404, $"{Open311Constants.ModelProperties.JurisdictionId} provided was not found");

            return result;
        }

        [HttpGet("services/{serviceCode}.{format}")]
        public async Task<IActionResult> GetServiceDefinition(GetServiceDefinitionInputModel model, CancellationToken cancellationToken)
        {
            var store = await _jurisdiction.GetServiceStore(model);
            var definition = await store.GetServiceDefinition(model.ServiceCode, cancellationToken);

            var result = definition != null
                ? Ok(definition)
                : NotFound(404, $"{Open311Constants.ModelProperties.ServiceCode} provided was not found.");

            return result;
        }
    }
}
