namespace Open311.GeoReportApi.Models
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = Open311Constants.DefaultNamespace)]
    public class ServiceAttributeValue
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
