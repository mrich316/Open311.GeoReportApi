namespace Open311.GeoReportApi.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ServiceDefinition
    {
        /// <summary>
        /// The unique identifier for the service request type.
        /// </summary>
        [Required]
        public string ServiceCode { get; set; }

        public List<ServiceAttribute> ServiceAttributes { get; set; }
    }
}
