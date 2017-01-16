namespace Open311.GeoReportApi.Controllers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using InputModels;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Services;

    [FormatFilter]
    [Route(Open311Constants.Routes.GeoReportV2)]
    public class RequestsController : GeoReportController
    {
        private readonly IJurisdictionService _jurisdiction;

        public RequestsController(IJurisdictionService jurisdiction)
        {
            if (jurisdiction == null) throw new ArgumentNullException(nameof(jurisdiction));
            _jurisdiction = jurisdiction;
        }

        [HttpGet("requests/{serviceRequestId}.{format}")]
        public async Task<IActionResult> GetServiceRequest(GetServiceRequestInputModel model)
        {
            var requestStore = await _jurisdiction.GetServiceRequestStore(model);
            var serviceRequest = await requestStore.Get(model.ServiceRequestId);

            return serviceRequest != null
                ? Ok(serviceRequest)
                : NotFound(404, $"{Open311Constants.ModelProperties.ServiceRequest} provided was not found.");
        }

        [HttpGet("requests.{format}")]
        public async Task<IActionResult> GetServiceRequests(GetServiceRequestsInputModel model)
        {
            var requestStore = await _jurisdiction.GetServiceRequestStore(model);

            // TODO: Implement GetServiceRequests.
            // TODO: Find a way to expose a clean search interface.
            var serviceRequests = await requestStore.Search();

            return Ok(new ServiceRequests<ServiceRequest>(serviceRequests));
        }

        [HttpPost("requests.{format}")]
        public async Task<IActionResult> PostServiceRequest(PostServiceRequestInputModel model)
        {
            // TODO: Requires API key.

            var serviceStore = await _jurisdiction.GetServiceStore(model);
            var service = await serviceStore.GetService(model.ServiceCode, CancellationToken.None);

            if (service == null)
            {
                return NotFound(404, $"{Open311Constants.ModelProperties.ServiceCode} was not found.");
            }

            if (service.Metadata)
            {
                // TODO: Validate attributes.
            }

            // TODO: Transform to entity, save.
            var requestStore = await _jurisdiction.GetServiceRequestStore(model);
            var created = await requestStore.Create();

            return Ok(new ServiceRequests<ServiceRequestCreated>(created));
        }
    }
}