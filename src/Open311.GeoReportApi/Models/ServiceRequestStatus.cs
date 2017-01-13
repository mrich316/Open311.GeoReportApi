namespace Open311.GeoReportApi.Models
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = Open311Constants.DefaultNamespace)]
    public enum ServiceRequestStatus
    {
        [EnumMember(Value = "closed")]
        Closed,

        [EnumMember(Value = "open")]
        Open
    }
}
