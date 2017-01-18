namespace Open311.GeoReportApi.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract(Name = Open311Constants.ModelProperties.ServiceDefinition, Namespace = Open311Constants.DefaultNamespace)]
    public class ServiceDefinition
    {
        /// <summary>
        /// The unique identifier for the service request type.
        /// </summary>
        [Required]
        [Display(Name = Open311Constants.ModelProperties.ServiceCode)]
        public string ServiceCode { get; set; }

        public List<ServiceAttribute> ServiceAttributes { get; set; }
    }
}
