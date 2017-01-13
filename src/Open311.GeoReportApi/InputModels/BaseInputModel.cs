namespace Open311.GeoReportApi.InputModels
{
    using System.ComponentModel.DataAnnotations;

    public class BaseInputModel
    {
        public BaseInputModel()
        {
            JurisdictionId = Open311Options.DefaultJurisdictionId;
        }

        /// <summary>
        /// As a means to allow this API to distinguish multiple jurisdictions within one API endpoint we’ve included a jurisdiction id
        /// which will be the unique identifier for cities. It has been suggested that we use the jurisdiction’s main website root url
        /// (without the www) as the jurisdiction id. For San Francisco, the jurisdiction_id is sfgov.org.
        /// Implementations can ignore this parameter and treat it as an "Optional Argument"
        /// if the implementation only serves one jurisdiction.
        /// </summary>
        [Required]
        [Display(Name = Open311Constants.ModelProperties.JurisdictionId)]
        public string JurisdictionId { get; set; }
    }
}