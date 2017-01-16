namespace Open311.GeoReportApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Models;

    public abstract class GeoReportController : Controller
    {
        public virtual IActionResult NotFound(int code, string description)
        {
            return NotFound(new Error
            {
                Code = code,
                Description = description
            });
        }
    }
}
