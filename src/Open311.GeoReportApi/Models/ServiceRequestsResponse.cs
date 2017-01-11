namespace Open311.GeoReportApi.Models
{
    using System.Collections.Generic;

    public class ServiceRequestsResponse<T>
    {
        public ServiceRequestsResponse()
        {
            ServiceRequests = new List<T>();
        }

        public ServiceRequestsResponse(params T[] serviceRequests)
        {
            ServiceRequests = new List<T>(serviceRequests);
        }

        public List<T> ServiceRequests { get; set; }
    }
}
