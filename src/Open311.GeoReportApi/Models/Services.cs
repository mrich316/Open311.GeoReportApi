namespace Open311.GeoReportApi.Models
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [CollectionDataContract(Name = "services", ItemName = "service")]
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