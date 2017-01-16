namespace Open311.GeoReportApi.InputModels
{
    using Models;

    public class GetServiceRequestIdInputModel : BaseInputModel
    {
        /// <summary>
        /// Token from <see cref="ServiceRequestCreated"/>.
        /// </summary>
        public string Token { get; set; }
    }
}
