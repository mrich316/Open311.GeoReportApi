namespace Open311.GeoReportApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    [DataContract(Name = Open311Constants.ModelProperties.Service, Namespace = Open311Constants.DefaultNamespace)]
    public class Service
    {
        public Service()
        {
            Attributes = new ServiceAttributes();
            Keywords = new List<string>();
        }

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
        public bool Metadata
        {
            get { return Attributes.Any(); }

#if NETSTANDARD_DOES_NOT_SERIALIZE_READ_ONLY_TYPES_BUG
            internal set
            {
                throw new NotSupportedException(
                    "Enabled only because DataContractSerializer does not honor SerializeReadOnlyTypes in netstandard, see https://github.com/mrich316/Open311.GeoReportApi/issues/1");
            }
#endif
        }

        /// <summary>
        /// Service type.
        /// </summary>
        [DataMember(Name = Open311Constants.ModelProperties.Type)]
        public ServiceType Type { get; set; }

        /// <summary>
        /// A comma separated list of tags or keywords to help users identify the request type.
        /// This can provide synonyms of the service_name and group.
        /// </summary>
        [IgnoreDataMember]
        public List<string> Keywords { get; set; }

        [DataMember(Name = Open311Constants.ModelProperties.Keywords)]
        internal string KeywordStrings
        {
            get { return string.Join(",", Keywords); }

#if NETSTANDARD_DOES_NOT_SERIALIZE_READ_ONLY_TYPES_BUG
            set
            {
                throw new NotSupportedException(
                    "Enabled only because DataContractSerializer does not honor SerializeReadOnlyTypes in netstandard, see https://github.com/mrich316/Open311.GeoReportApi/issues/1");
            }
#endif
        }

        /// <summary>
        /// A category to group this service type within. This provides a way to group
        /// several service request types under one category such as "sanitation".
        /// </summary>
        [DataMember(Name = Open311Constants.ModelProperties.Group)]
        public string Group { get; set; }

        [IgnoreDataMember]
        public ServiceAttributes Attributes { get; set; }
    }
}
