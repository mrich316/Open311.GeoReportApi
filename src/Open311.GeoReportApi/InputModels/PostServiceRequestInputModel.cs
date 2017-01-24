namespace Open311.GeoReportApi.InputModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class PostServiceRequestInputModel : BaseInputModel, IValidatableObject
    {
        /// <summary>
        /// The unique identifier for the service request type.
        /// </summary>
        [Required]
        [Display(Name = Open311Constants.ModelProperties.ServiceCode)]
        public string ServiceCode { get; set; }

        /// <summary>
        /// An array of key/value responses based on Service Definitions.
        /// </summary>
        /// <remarks>
        /// This takes the form of attribute[code]=value where multiple code/value pairs can be specified as well
        /// as multiple values for the same code in the case of a multivaluelist datatype
        /// (attribute[code1][]=value1&amp;attribute[code1][]=value2&amp;attribute[code1][]=value3).
        /// This is only required if the service_code requires a service definition with required fields`.
        /// </remarks>
        public Dictionary<string, string> Attribute { get; set; }

        /// <summary>
        /// Latitude using the (WGS84) projection.
        /// </summary>
        /// <remarks>
        /// <see cref="Lat"/> and <see cref="Long"/> both need to be sent even though they are sent as two separate parameters.
        /// <see cref="Lat"/> and <see cref="Long"/> are required if no <see cref="AddressString"/> or <see cref="AddressId"/> is provided.
        /// </remarks>
        [Range(-90, 90)]
        [Display(Name = Open311Constants.ModelProperties.Lat)]
        public double? Lat { get; set; }

        /// <summary>
        /// Longitude using the (WGS84) projection.
        /// </summary>
        /// <remarks>
        /// <see cref="Lat"/> and <see cref="Long"/> both need to be sent even though they are sent as two separate parameters.
        /// <see cref="Lat"/> and <see cref="Long"/> are required if no <see cref="AddressString"/> or <see cref="AddressId"/> is provided.
        /// </remarks>
        [Range(-180, 180)]
        [Display(Name = Open311Constants.ModelProperties.Long)]
        public double? Long { get; set; }

        /// <summary>
        /// Human entered address or description of location.
        /// </summary>
        /// <remarks>
        /// This is required if no <see cref="Lat"/>/<see cref="Long"/> or <see cref="AddressId"/> are provided.
        /// This should be written from most specific to most general geographic unit,
        /// eg: address number or cross streets, street name, neighborhood/district, city/town/village, county, postal code.
        /// </remarks>
        public string AddressString { get; set; }

        /// <summary>
        /// The internal address ID used by a jurisdiction’s master address repository or other addressing system.
        /// </summary>
        /// <remarks>
        /// This is required if no <see cref="Lat"/>/<see cref="Long"/> or <see cref="AddressString"/> are provided.
        /// </remarks>
        public string AddressId { get; set; }

        /// <summary>
        /// The email address of the person submitting the request.
        /// </summary>
        [EmailAddress]
        [Display(Name = Open311Constants.ModelProperties.Email)]
        public string Email { get; set; }

        /// <summary>
        /// The unique device ID of the device submitting the request.
        /// This is usually only used for mobile devices.
        /// </summary>
        /// <remarks>
        /// Android devices can use TelephonyManager.getDeviceId() and iPhones can use [UIDevice currentDevice].uniqueIdentifier
        /// </remarks>
        public string DeviceId { get; set; }

        /// <summary>
        /// The unique ID for the user account of the person submitting the request.
        /// </summary>
        public string AccountId { get; set; }

        /// <summary>
        /// The given name of the person submitting the request.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The family name of the person submitting the request.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The phone number of the person submitting the request.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// A full description of the request or report being submitted.
        /// This may contain line breaks, but not html or code. Otherwise,
        /// this is free form text limited to 4,000 characters.
        /// </summary>
        [MaxLength(4000)]
        [Display(Name = Open311Constants.ModelProperties.Description)]
        public string Description { get; set; }

        /// <summary>
        /// A URL to media associated with the request, eg an image.
        /// </summary>
        /// <remarks>
        /// A convention for parsing media from this URL has yet to be established,
        /// so currently it will be done on a case by case basis much like Twitter.com does.
        /// For example, if a jurisdiction accepts photos submitted via Twitpic.com, then clients
        /// can parse the page at the Twitpic URL for the image given the conventions of Twitpic.com.
        /// This could also be a URL to a media RSS feed where the clients can
        /// parse for media in a more structured way.
        /// </remarks>
        [Url]
        [Display(Name = Open311Constants.ModelProperties.MediaUrl)]
        public Uri MediaUrl { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Lat.HasValue && !Long.HasValue
                || !Lat.HasValue && Long.HasValue)
            {
                yield return new ValidationResult(
                    "Missing latitude or longitude. Ensure both (or none) are provided.");
            }

            if (!Lat.HasValue && !Long.HasValue
                && AddressId == null && AddressString == null)
            {
                yield return
                    new ValidationResult(
                        "Missing location parameters. Ensure at least one location parameter is defined (address string, address id or latitude/longitude).");
            }
        }
    }
}