namespace Open311.GeoReportApi.InputModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using ModelBinding;
    using Models;

    public class GetServiceRequestsInputModel : BaseInputModel, IValidatableObject
    {
        public GetServiceRequestsInputModel()
        {
            // Defaults to last 90 days.
            EndDate = DateTimeOffset.UtcNow;
            StartDate = EndDate - TimeSpan.FromDays(90);
        }

        /// <summary>
        /// To call multiple Service Requests at once, multiple service_request_id can be declared, command delimited.
        /// This overrides all other arguments.
        /// </summary>
        [ModelBinder(BinderType = typeof(CommaDelimitedListModelBinder))]
        public List<string> ServiceRequestId { get; set; }

        /// <summary>
        /// Specify the service type by calling the unique ID of the service_code.
        /// This defaults to all service codes when not declared; can be declared multiple times, comma delimited
        /// </summary>
        [ModelBinder(BinderType = typeof(CommaDelimitedListModelBinder))]
        public List<string> ServiceCode { get; set; }

        /// <summary>
        /// Earliest datetime to include in search. When provided with end_date, allows one to search for requests
        /// which have a requested_datetime that matches a given range, but may not span more than 90 days.
        /// </summary>
        [Display(Name = "start_date")]
        public DateTimeOffset? StartDate { get; set; }

        /// <summary>
        /// Earliest datetime to include in search. When provided with end_date, allows one to search for requests
        /// which have a requested_datetime that matches a given range, but may not span more than 90 days.
        /// </summary>
        [Display(Name = "end_date")]
        public DateTimeOffset? EndDate { get; set; }

        /// <summary>
        /// Allows one to search for requests which have a specific status.
        /// This defaults to all statuses; can be declared multiple times, comma delimited;
        /// </summary>
        [ModelBinder(BinderType = typeof(CommaDelimitedListModelBinder))]
        public List<ServiceRequestStatus> Status { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // TODO: EndDate - StartDate must be lower than or equal to 90 days.
            return Enumerable.Empty<ValidationResult>();
        }
    }
}
