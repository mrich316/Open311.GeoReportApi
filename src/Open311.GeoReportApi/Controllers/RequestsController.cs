namespace Open311.GeoReportApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        private readonly IServiceAttributeValidator _attributeValidator;

        public RequestsController(IJurisdictionService jurisdiction, IServiceAttributeValidator attributeValidator)
        {
            if (jurisdiction == null) throw new ArgumentNullException(nameof(jurisdiction));
            if (attributeValidator == null) throw new ArgumentNullException(nameof(attributeValidator));

            _jurisdiction = jurisdiction;
            _attributeValidator = attributeValidator;
        }

        [HttpGet("requests/{serviceRequestId}.{format}")]
        public async Task<IActionResult> GetServiceRequest(GetServiceRequestInputModel model, CancellationToken cancellationToken)
        {
            var searchService = await _jurisdiction.GetServiceRequestSearchService(model);
            var serviceRequest = (await searchService.Get(new[] {model.ServiceRequestId}, cancellationToken))
                .FirstOrDefault();

            return serviceRequest != null
                ? Ok(new ServiceRequests<ServiceRequest>(serviceRequest))
                : NotFound(404, $"{Open311Constants.ModelProperties.ServiceRequest} provided was not found.");
        }

        [HttpGet("requests.{format}")]
        public async Task<IActionResult> GetServiceRequests(GetServiceRequestsInputModel model, CancellationToken cancellationToken)
        {
            IEnumerable<ServiceRequest> serviceRequests;
            var searchService = await _jurisdiction.GetServiceRequestSearchService(model);

            // if a service request id is defined, it overrides all other arguments.
            if (model.ServiceRequestId.Any())
            {
                serviceRequests = await searchService.Get(model.ServiceRequestId, cancellationToken);
            }
            else
            {
                var query = new ServiceRequestQuery
                {
                    EndDate = model.EndDate,
                    ServiceCodes = model.ServiceCode,
                    StartDate = model.StartDate,
                    Statuses = model.Status
                };

                serviceRequests = await searchService.Search(query, cancellationToken);
            }

            return Ok(new ServiceRequests<ServiceRequest>(serviceRequests));
        }

        [HttpPost("requests.{format}")]
        public async Task<IActionResult> PostServiceRequest(PostServiceRequestInputModel model)
        {
            // TODO: Requires API key. Can we use AuthorizeAttribute ?

            var serviceStore = await _jurisdiction.GetServiceStore(model);
            var service = await serviceStore.GetService(model.ServiceCode, CancellationToken.None);

            if (service == null)
            {
                return NotFound(404,
                    $"{Open311Constants.ModelProperties.ServiceCode} was not found in jurisdiction.");
            }

            var validationErrors = await _attributeValidator.ValidateMetadata(service, model);
            if (validationErrors != null && validationErrors.Any())
            {
                var attributeErrors = validationErrors.Select(e => new Error {Code = 400, Description = e.ErrorMessage});
                return BadRequest(new Errors(attributeErrors));
            }

            var created = await serviceStore.Create(model);

            return Ok(new ServiceRequests<ServiceRequestCreated>(created));
        }
    }
}