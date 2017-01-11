namespace Open311.GeoReportApi.InputModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class GetServiceDefinitionInputModel : BaseInputModel
    {
        /// <summary>
        /// The unique identifier for the service request type.
        /// </summary>
        [Required]
        [DataMember(Name = "service_code")]
        [Display(Name = "service_code")]
        public string ServiceCode { get; set; }
    }
}