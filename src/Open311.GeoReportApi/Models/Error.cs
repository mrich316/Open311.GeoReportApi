namespace Open311.GeoReportApi.Models
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = Open311Constants.DefaultNamespace)]
    public class Error
    {
        /// <summary>
        /// The error code representing the type of error.
        /// In most cases, this should match the HTTP status code returned in the HTTP header.
        /// </summary>
        [DataMember(Name = Open311Constants.ModelProperties.Code)]
        public int Code { get; set; }

        /// <summary>
        /// A human readable description of the error that occurred.
        /// This is meant to be seen by the user.
        /// </summary>
        [DataMember(Name = Open311Constants.ModelProperties.Description)]
        public string Description { get; set; }
    }
}
