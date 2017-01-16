namespace Open311.GeoReportApi.Models
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [CollectionDataContract(
        Name = Open311Constants.ModelProperties.ServiceAttributes,
        ItemName = Open311Constants.ModelProperties.ServiceAttribute,
        Namespace = Open311Constants.DefaultNamespace)]
    public class ServiceAttributes : List<ServiceAttribute>
    {
        public ServiceAttributes()
        {
        }

        public ServiceAttributes(IEnumerable<ServiceAttribute> collection)
            : base(collection)
        {
        }

        public ServiceAttributes(int capacity)
            : base(capacity)
        {
        }
    }
}
