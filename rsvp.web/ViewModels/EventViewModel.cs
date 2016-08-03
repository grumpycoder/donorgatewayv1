using DonorGateway.Domain;
using System;

namespace rsvp.web.ViewModels
{
    public class EventViewModel
    {
        public string PromoCode { get; set; }

        public string Speaker { get; set; }
        public string Venue { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? VenueOpenDate { get; set; }
        public int Capacity { get; set; }
        public int? TicketAllowance { get; set; }
        public bool IsCancelled { get; set; }

        public int? TemplateId { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string EventDisplayName { get; set; }

        public int GuestWaitingCount { get; set; }
        public int GuestAttendanceCount { get; set; }

        public bool IsExpired => EndDate < DateTime.Now;
        public bool IsAtCapacity => TicketRemainingCount <= 0;
        public int TicketRemainingCount => Capacity - GuestAttendanceCount;

        public Template Template { get; set; }
    }
}

