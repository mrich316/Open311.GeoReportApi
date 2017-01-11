namespace Open311.GeoReportApi.Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public class Service
    {
        /// <summary>
        /// The unique identifier for the service request type.
        /// </summary>
        [DataMember(Name = "service_code")]
        public string ServiceCode { get; set; }

        /// <summary>
        /// The human readable name of the service request type.
        /// </summary>
        [DataMember(Name = "service_name")]
        public string ServiceName { get; set; }

        /// <summary>
        /// A brief description of the service request type.
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }

        /// <summary>
        /// Determines whether there are additional form fields for this service type.
        /// <c>true</c>: This service request type requires additional metadata so the client
        /// will need to make a call to the Service Definition method.
        /// <c>false</c>: No additional information is required and a call to the Service
        /// Definition method is not needed.
        /// </summary>
        [DataMember(Name = "metadata")]
        public bool Metadata { get; set; }

        /// <summary>
        /// Service type.
        /// </summary>
        [DataMember(Name = "type")]
        public ServiceType Type { get; set; }

        /// <summary>
        /// A comma separated list of tags or keywords to help users identify the request type.
        /// This can provide synonyms of the service_name and group.
        /// </summary>
        [DataMember(Name = "keywords")]
        public string Keywords { get; set; }

        /// <summary>
        /// A category to group this service type within. This provides a way to group
        /// several service request types under one category such as "sanitation".
        /// </summary>
        [DataMember(Name = "group")]
        public string Group { get; set; }
    }
}
