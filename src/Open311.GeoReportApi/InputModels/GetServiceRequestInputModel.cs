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
        [DataMember(Name = Open311Constants.ModelProperties.ServiceRequestId)]
        [Display(Name = Open311Constants.ModelProperties.ServiceRequestId)]
        public string ServiceRequestId { get; set; }
    }
}
