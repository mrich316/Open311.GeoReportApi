namespace Open311.GeoReportApi.Models
{
    using System.Runtime.Serialization;

    /// <summary>
    /// <c>realtime</c>: The service request ID will be returned immediately after the service request is submitted.
    /// <c>batch</c>: A token will be returned immediately after the service request is submitted.
    /// This token can then be later used to return the service request ID.
    /// <c>blackbox</c>: No service request ID will be returned after the service request is submitted
    /// </summary>
    [DataContract(Name = Open311Constants.ModelProperties.ServiceType, Namespace = Open311Constants.DefaultNamespace)]
    public enum ServiceType
    {
        /// <summary>
        /// The service request ID will be returned immediately after the service request is submitted. 
        /// </summary>
        [EnumMember(Value = "realtime")]
        Realtime,

        /// <summary>
        /// A token will be returned immediately after the service request is submitted.
        /// This token can then be later used to return the service request ID.
        /// </summary>
        [EnumMember(Value = "batch")]
        Batch,

        /// <summary>
        /// No service request ID will be returned after the service request is submitted.
        /// </summary>
        [EnumMember(Value = "blackbox")]
        Blackbox
    }
}
