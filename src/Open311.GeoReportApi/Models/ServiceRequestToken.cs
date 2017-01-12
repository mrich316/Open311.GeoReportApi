namespace Open311.GeoReportApi.Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ServiceRequestToken
    {
        /// <summary>
        /// The unique ID for the service request created. This can be used to call the GET Service Request method.
        /// </summary>
        public string ServiceRequestId { get; set; }

        /// <summary>
        /// The token ID used to make this call.
        /// </summary>
        public string Token { get; set; }
    }
}
