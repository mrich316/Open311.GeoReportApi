namespace Open311.GeoReportApi.InputModels
{
    using System.Runtime.Serialization;

    [DataContract]
    public class GetServiceRequestIdInputModel : BaseInputModel
    {
        /// <summary>
        /// Token from <see cref="Models.PostServiceRequestResponse"/>.
        /// </summary>
        public string Token { get; set; }
    }
}
