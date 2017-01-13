namespace Open311.GeoReportApi.InputModels
{
    using System.ComponentModel.DataAnnotations;

    public class GetServiceDefinitionInputModel : BaseInputModel
    {
        /// <summary>
        /// The unique identifier for the service request type.
        /// </summary>
        [Required]
        [Display(Name = Open311Constants.ModelProperties.ServiceCode)]
        public string ServiceCode { get; set; }
    }
}