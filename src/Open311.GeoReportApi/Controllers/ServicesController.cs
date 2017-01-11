namespace Open311.GeoReportApi.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using InputModels;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Services;

    [FormatFilter]
    [Route("")]
    public class ServicesController : Controller
    {
        private readonly IServiceStore _serviceStore;

        public ServicesController(IServiceStore serviceStore)
        {
            if (serviceStore == null) throw new ArgumentNullException(nameof(serviceStore));
            _serviceStore = serviceStore;
        }

        [HttpGet("services.{format}")]
        public async Task<IActionResult> GetServiceList(GetServiceListInputModel model)
        {
            var services = await _serviceStore
                .GetServices(model.JurisdictionId);

            var serviceList = new Services<Service>(services);

            if (!serviceList.Any())
            {
                return NotFound(new Error
                {
                    Code = 404,
                    Description = "jurisdiction_id provided was not found"
                });
            }

            return Ok(serviceList);
        }
    }
}
