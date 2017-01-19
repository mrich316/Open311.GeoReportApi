namespace Open311.GeoReportApi.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using InputModels;
    using Models;
    using System.Globalization;

    public class DefaultServiceAttributeValidator : IServiceAttributeValidator
    {
        private static readonly Task<List<ValidationResult>> WithoutErrors = Task.FromResult(new List<ValidationResult>());

        public Task<List<ValidationResult>> ValidateMetadata(Service service,
            PostServiceRequestInputModel serviceRequest)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            if (!service.Metadata) return WithoutErrors;

            if (serviceRequest == null) throw new ArgumentNullException(nameof(serviceRequest));

            var results = new List<ValidationResult>();
            var requestAttributes = serviceRequest.Attribute ?? new Dictionary<string, string>();

            foreach (var serviceAttribute in service.Attributes)
            {
                if (requestAttributes.ContainsKey(serviceAttribute.Code))
                {
                    var value = requestAttributes[serviceAttribute.Code];

                    ValidateServiceAttribute(serviceAttribute, value, results);
                }
                else if (serviceAttribute.Required)
                {
                    results.Add(new ValidationResult($"attribute[{serviceAttribute.Code}] is required."));
                }
            }

            return results.Any()
                ? Task.FromResult(results)
                : WithoutErrors;
        }

        public virtual void ValidateServiceAttribute(ServiceAttribute attribute, string value, List<ValidationResult> validationResults)
        {
            if (attribute == null) throw new ArgumentNullException(nameof(attribute));
            if (validationResults == null) throw new ArgumentNullException(nameof(validationResults));

            switch (attribute.Datatype)
            {
                case ServiceAttributeDatatype.String:
                case ServiceAttributeDatatype.Text:

                    break;

                case ServiceAttributeDatatype.Singlevaluelist:

                    if (!attribute.Values.Contains(new ServiceAttributeValue(value ?? string.Empty)))
                    {
                        var options = string.Join("', '", attribute.Values);
                        validationResults.Add(new ValidationResult(
                            $"invalid option for attribute[{attribute.Code}]. Valid options are: '{options}'."));
                    }

                    break;

                case ServiceAttributeDatatype.Number:

                    double numberValue;

                    if (!double.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out numberValue))
                    {
                        validationResults.Add(new ValidationResult(
                            $"invalid number for attribute[{attribute.Code}]."));
                    }

                    break;

                case ServiceAttributeDatatype.Multivaluelist:

                    var values = (value ?? string.Empty)
                        .Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries)
                        .Select(v => new ServiceAttributeValue(v));

                    if (values.Any(v => !attribute.Values.Contains(new ServiceAttributeValue(value))))
                    {
                        var options = string.Join("', '", attribute.Values);
                        validationResults.Add(new ValidationResult(
                            $"invalid option for attribute[{attribute.Code}]. Valid options are: '{options}'."));
                    }

                    break;

                case ServiceAttributeDatatype.Datetime:

                    DateTimeOffset dateValue;

                    if (!DateTimeOffset.TryParseExact(value, "o", // iso-8601 / Round-trip
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.AssumeUniversal | DateTimeStyles.RoundtripKind,
                        out dateValue))
                    {
                        validationResults.Add(new ValidationResult(
                            $"invalid datetime for attribute[{attribute.Code}]. The value must be iso-8601 compliant."));
                    }

                    break;

                default:
                    throw new NotSupportedException();
            }
        }
    }
}