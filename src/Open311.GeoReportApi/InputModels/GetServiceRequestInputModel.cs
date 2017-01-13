namespace Open311.GeoReportApi.InputModels
{
    using System.ComponentModel.DataAnnotations;

    public class GetServiceRequestInputModel : BaseInputModel
    {
        /// <summary>
        /// The unique ID of the service request.
        /// </summary>
        [Required]
        [Display(Name = Open311Constants.ModelProperties.ServiceRequestId)]
        public string ServiceRequestId { get; set; }
    }
}
