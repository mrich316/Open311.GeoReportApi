namespace Open311.GeoReportApi.InputModels
{
    public class GetServiceRequestIdInputModel : BaseInputModel
    {
        /// <summary>
        /// Token from <see cref="Models.ServiceRequestCreated"/>.
        /// </summary>
        public string Token { get; set; }
    }
}
