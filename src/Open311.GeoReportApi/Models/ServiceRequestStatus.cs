namespace Open311.GeoReportApi.Models
{
    using System.Runtime.Serialization;

    [DataContract(Name = Open311Constants.ModelProperties.ServiceRequestStatus, Namespace = Open311Constants.DefaultNamespace)]
    public enum ServiceRequestStatus
    {
        [EnumMember(Value = "closed")]
        Closed,

        [EnumMember(Value = "open")]
        Open
    }
}
