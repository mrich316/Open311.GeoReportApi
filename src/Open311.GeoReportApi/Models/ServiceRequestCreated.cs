namespace Open311.GeoReportApi.Models
{
    using System.Runtime.Serialization;

    [DataContract(Name = Open311Constants.ModelProperties.ServiceRequestCreated, Namespace = Open311Constants.DefaultNamespace)]
    public class ServiceRequestCreated
    {
        /// <summary>
        /// The unique ID of the service request created.
        /// </summary>
        /// <remarks>
        /// This should not be returned if <see cref="Token"/>  is returned
        /// </remarks>
        [DataMember(Name = Open311Constants.ModelProperties.ServiceRequestId, EmitDefaultValue = false)]
        public string ServiceRequestId { get; set; }

        /// <summary>
        /// If returned, use this to call GET service_request_id from a token.
        /// </summary>
        /// <remarks>
        /// This should not be returned if <see cref="ServiceRequestId"/> is returned
        /// </remarks>
        [DataMember(Name = Open311Constants.ModelProperties.Token, EmitDefaultValue = false)]
        public string Token { get; set; }

        /// <summary>
        /// Information about the action expected to fulfill the request or otherwise address the information reported.
        /// May not be returned.
        /// </summary>
        [DataMember(Name = Open311Constants.ModelProperties.ServiceNotice, EmitDefaultValue = false)]
        public string ServiceNotice { get; set; }

        /// <summary>
        /// The unique ID for the user account of the person submitting the request.
        /// May not be returned.
        /// </summary>
        [DataMember(Name = Open311Constants.ModelProperties.AccountId, EmitDefaultValue = false)]
        public string AccountId { get; set; }
    }
}