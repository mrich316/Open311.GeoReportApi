namespace Open311.GeoReportApi.Controllers
{
    using System;
    using System.Threading.Tasks;
    using InputModels;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Services;

    [FormatFilter]
    [Route(Open311Constants.Routes.GeoReportV2)]
    public class TokensController : GeoReportController
    {
        private readonly IJurisdictionService _jurisdiction;

        public TokensController(IJurisdictionService jurisdiction)
        {
            if (jurisdiction == null) throw new ArgumentNullException(nameof(jurisdiction));
            _jurisdiction = jurisdiction;
        }

        [HttpGet("tokens/{token}.{format}")]
        public IActionResult GetServiceRequestId(GetServiceRequestIdInputModel model)
        {
            // TODO: Implement GetServiceRequestId.
            return Ok(new ServiceRequests<ServiceRequestToken>(new ServiceRequestToken
            {
                ServiceRequestId = new Random().Next().ToString(),
                Token = new Random().Next().ToString()
            }));
        }
    }
}
