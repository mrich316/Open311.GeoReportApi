namespace Open311.GeoReportApi.Services
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using InputModels;
    using Models;

    public interface IServiceAttributeValidator
    {
        // TODO: Not sure it's a good idea to expose an input model here (PostServiceRequestInputModel).
        Task<List<ValidationResult>> ValidateMetadata(Service service, PostServiceRequestInputModel serviceRequest);

        void ValidateServiceAttribute(ServiceAttribute attribute, string value, List<ValidationResult> validationResults);
    }
}
