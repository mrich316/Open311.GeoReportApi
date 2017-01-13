namespace Open311.GeoReportApi.Models
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [CollectionDataContract(
        Name = Open311Constants.ModelProperties.Services, 
        ItemName = Open311Constants.ModelProperties.Service,
        Namespace = Open311Constants.DefaultNamespace)]
    public class Services<T> : List<T>
    {
        public Services()
        {
        }

        public Services(int capacity)
            : base(capacity)
        {
        }

        public Services(IEnumerable<T> collection)
            : base(collection)
        {
        }
    }
}