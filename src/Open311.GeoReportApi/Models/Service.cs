namespace Open311.GeoReportApi.Models
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;

    [DataContract(Namespace = Open311Constants.DefaultNamespace)]
    public class Service
    {
        /// <summary>
        /// The unique identifier for the service request type.
        /// </summary>
        [DataMember(Name = Open311Constants.ModelProperties.ServiceCode)]
        public string ServiceCode { get; set; }

        /// <summary>
        /// The human readable name of the service request type.
        /// </summary>
        [DataMember(Name = Open311Constants.ModelProperties.ServiceName)]
        public string ServiceName { get; set; }

        /// <summary>
        /// A brief description of the service request type.
        /// </summary>
        [DataMember(Name = Open311Constants.ModelProperties.Description)]
        public string Description { get; set; }

        /// <summary>
        /// Determines whether there are additional form fields for this service type.
        /// <c>true</c>: This service request type requires additional metadata so the client
        /// will need to make a call to the Service Definition method.
        /// <c>false</c>: No additional information is required and a call to the Service
        /// Definition method is not needed.
        /// </summary>
        [DataMember(Name = Open311Constants.ModelProperties.Metadata)]
        public bool Metadata { get; set; }

        /// <summary>
        /// Service type.
        /// </summary>
        [DataMember(Name = Open311Constants.ModelProperties.Type)]
        public ServiceType Type { get; set; }

        /// <summary>
        /// A comma separated list of tags or keywords to help users identify the request type.
        /// This can provide synonyms of the service_name and group.
        /// </summary>
        [DataMember(Name = Open311Constants.ModelProperties.Keywords)]
        public string Keywords { get; set; }

        /// <summary>
        /// A category to group this service type within. This provides a way to group
        /// several service request types under one category such as "sanitation".
        /// </summary>
        [DataMember(Name = Open311Constants.ModelProperties.Group)]
        public string Group { get; set; }

        [IgnoreDataMember]
        [JsonIgnore]
        public Dictionary<string, ServiceAttribute> ServiceAttributes { get; set; }
    }
}
