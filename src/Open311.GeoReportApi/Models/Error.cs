namespace Open311.GeoReportApi.Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public class Error
    {
        /// <summary>
        /// The error code representing the type of error.
        /// In most cases, this should match the HTTP status code returned in the HTTP header.
        /// </summary>
        [DataMember(Name = "code")]
        public int Code { get; set; }

        /// <summary>
        /// A human readable description of the error that occurred.
        /// This is meant to be seen by the user.
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }
    }
}
