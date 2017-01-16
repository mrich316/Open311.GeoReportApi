namespace Open311.GeoReportApi.Models
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [CollectionDataContract(
        Name = Open311Constants.ModelProperties.ServiceRequests,
        ItemName = Open311Constants.ModelProperties.ServiceRequest,
        Namespace = Open311Constants.DefaultNamespace)]
    public class ServiceRequests<T> : List<T>
    {
        public ServiceRequests()
        {
        }

        public ServiceRequests(int capacity)
            : base(capacity)
        {
        }

        public ServiceRequests(params T[] serviceRequests)
            : base(serviceRequests)
        {
        }

        public ServiceRequests(IEnumerable<T> serviceRequests)
            : base(serviceRequests)
        {
        }
    }
}