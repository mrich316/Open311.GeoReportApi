namespace Open311.GeoReportApi.Services
{
    using System;
    using System.Collections.Generic;
    using Models;

    public class ServiceRequestQuery
    {
        /// <summary>
        /// Specify the service type by calling the unique ID of the service_code.
        /// This defaults to all service codes when not declared; can be declared multiple times, comma delimited
        /// </summary>
        public List<string> ServiceCodes { get; set; }

        /// <summary>
        /// Earliest datetime to include in search. When provided with end_date, allows one to search for requests
        /// which have a requested_datetime that matches a given range, but may not span more than 90 days.
        /// </summary>
        public DateTimeOffset? StartDate { get; set; }

        /// <summary>
        /// Earliest datetime to include in search. When provided with end_date, allows one to search for requests
        /// which have a requested_datetime that matches a given range, but may not span more than 90 days.
        /// </summary>
        public DateTimeOffset? EndDate { get; set; }

        /// <summary>
        /// Allows one to search for requests which have a specific status.
        /// This defaults to all statuses; can be declared multiple times, comma delimited;
        /// </summary>
        public List<ServiceRequestStatus> Statuses { get; set; }
    }
}
