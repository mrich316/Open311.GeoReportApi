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
        [DataMember(Name = Open311Constants.ModelProperties.ServiceCode)]
        [Display(Name = Open311Constants.ModelProperties.ServiceCode)]
        public string ServiceCode { get; set; }
    }
}