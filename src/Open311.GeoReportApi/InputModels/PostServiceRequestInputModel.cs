namespace Open311.GeoReportApi.InputModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Runtime.Serialization;

    [DataContract]
    public class PostServiceRequestInputModel : BaseInputModel, IValidatableObject
    {
        /// <summary>
        /// The unique identifier for the service request type.
        /// </summary>
        [Required]
        [DataMember(Name = "service_code")]
        [Display(Name = "service_code")]
        public string ServiceCode { get; set; }

        // TODO: Attribute: An array of key/value responses based on Service Definitions.

        /// <summary>
        /// Latitude using the (WGS84) projection.
        /// </summary>
        /// <remarks>
        /// <see cref="Lat"/> and <see cref="Long"/> both need to be sent even though they are sent as two separate parameters.
        /// <see cref="Lat"/> and <see cref="Long"/> are required if no <see cref="AddressString"/> or <see cref="AddressId"/> is provided.
        /// </remarks>
        [DataMember(Name = "lat")]
        public double? Lat { get; set; }

        /// <summary>
        /// Longitude using the (WGS84) projection.
        /// </summary>
        /// <remarks>
        /// <see cref="Lat"/> and <see cref="Long"/> both need to be sent even though they are sent as two separate parameters.
        /// <see cref="Lat"/> and <see cref="Long"/> are required if no <see cref="AddressString"/> or <see cref="AddressId"/> is provided.
        /// </remarks>
        [DataMember(Name = "long")]
        public double? Long { get; set; }

        /// <summary>
        /// Human entered address or description of location.
        /// </summary>
        /// <remarks>
        /// This is required if no <see cref="Lat"/>/<see cref="Long"/> or <see cref="AddressId"/> are provided.
        /// This should be written from most specific to most general geographic unit,
        /// eg: address number or cross streets, street name, neighborhood/district, city/town/village, county, postal code.
        /// </remarks>
        [DataMember(Name = "address_string")]
        public string AddressString { get; set; }

        /// <summary>
        /// The internal address ID used by a jurisdiction’s master address repository or other addressing system.
        /// </summary>
        /// <remarks>
        /// This is required if no <see cref="Lat"/>/<see cref="Long"/> or <see cref="AddressString"/> are provided.
        /// </remarks>
        [DataMember(Name = "address_id")]
        public string AddressId { get; set; }

        /// <summary>
        /// The email address of the person submitting the request.
        /// </summary>
        [EmailAddress]
        [DataMember(Name = "email")]
        [Display(Name = "email")]
        public string Email { get; set; }

        /// <summary>
        /// The unique device ID of the device submitting the request.
        /// This is usually only used for mobile devices.
        /// </summary>
        /// <remarks>
        /// Android devices can use TelephonyManager.getDeviceId() and iPhones can use [UIDevice currentDevice].uniqueIdentifier
        /// </remarks>
        [DataMember(Name = "device_id")]
        public string DeviceId { get; set; }

        /// <summary>
        /// The unique ID for the user account of the person submitting the request.
        /// </summary>
        [DataMember(Name = "account_id")]
        public string AccountId { get; set; }

        /// <summary>
        /// The given name of the person submitting the request.
        /// </summary>
        [DataMember(Name = "first_name")]
        public string FirstName { get; set; }

        /// <summary>
        /// The family name of the person submitting the request.
        /// </summary>
        [DataMember(Name = "last_name")]
        public string LastName { get; set; }

        /// <summary>
        /// The phone number of the person submitting the request.
        /// </summary>
        [DataMember(Name = "phone")]
        public string Phone { get; set; }

        /// <summary>
        /// A full description of the request or report being submitted.
        /// This may contain line breaks, but not html or code. Otherwise,
        /// this is free form text limited to 4,000 characters.
        /// </summary>
        [MaxLength(4000)]
        [DataMember(Name = "description")]
        [Display(Name = "description")]
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
        [DataMember(Name = "media_url")]
        [Display(Name = "media_url")]
        public string MediaUrl { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // TODO: A full location parameter must be submitted.
            // TODO: One of lat & long or address_string or address_id.

            return Enumerable.Empty<ValidationResult>();
        }
    }
}