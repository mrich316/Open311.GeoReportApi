namespace Open311.GeoReportApi.InputModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class GetServiceRequestInputModel : BaseInputModel
    {
        /// <summary>
        /// The unique ID of the service request.
        /// </summary>
        [Required]
        [DataMember(Name = "service_request_id")]
        [Display(Name = "service_request_id")]
        public string ServiceRequestId { get; set; }
    }
}
