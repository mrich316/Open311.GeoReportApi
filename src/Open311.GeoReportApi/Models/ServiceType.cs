namespace Open311.GeoReportApi.Models
{
    /// <summary>
    /// <c>realtime</c>: The service request ID will be returned immediately after the service request is submitted.
    /// <c>batch</c>: A token will be returned immediately after the service request is submitted.
    /// This token can then be later used to return the service request ID.
    /// <c>blackbox</c>: No service request ID will be returned after the service request is submitted
    /// </summary>
    public enum ServiceType
    {
        Realtime,
        Batch,
        Blackbox
    }
}
