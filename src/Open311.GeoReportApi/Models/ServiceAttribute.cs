namespace Open311.GeoReportApi.Models
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract(Name = Open311Constants.ModelProperties.ServiceAttribute, Namespace = Open311Constants.DefaultNamespace)]
    public class ServiceAttribute
    {
        public ServiceAttribute()
        {
            ServiceAttributeValues = new HashSet<ServiceAttributeValue>();
        }

        /// <summary>
        ///     <c>true</c> denotes that user input is needed
        ///     <c>false</c> means the attribute is only used to present information to the user within the description field
        /// </summary>
        public bool Variable { get; set; }

        /// <summary>
        ///     A unique identifier for the attribute
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///     Denotes the type of field used for user input.
        /// </summary>
        public ServiceAttributeDatatype Datatype { get; set; }

        /// <summary>
        ///     <c>true</c> means that the value is required to submit service request
        ///     <c>false</c> means that the value not required.
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        ///     A description of the datatype which helps the user provide their input
        /// </summary>
        public string DatatypeDescription { get; set; }

        /// <summary>
        ///     The sort order that the attributes will be presented to the user. 1 is shown first in the list.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        ///     An description of the attribute field with instructions for the user to find and identify the requested information
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Options for <see cref="ServiceAttributeDatatype.Singlevaluelist" />
        ///     or <see cref="ServiceAttributeDatatype.Multivaluelist" />.
        ///     This is analogous to the select input in an html page.
        /// </summary>
        public HashSet<ServiceAttributeValue> ServiceAttributeValues { get; set; }
    }
}
