using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DonorGateway.Domain
{
    public class Event : BaseEntity
    {
        public Event()
        {
            Guests = new List<Guest>();
        }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string EventCode { get; set; }
        public string Speaker { get; set; }
        public string Venue { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public int Capacity { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? VenueOpenDate { get; set; }
        public DateTime? RegistrationCloseDate { get; set; }
        public int? TicketAllowance { get; set; }
        public bool? IsCancelled { get; set; }
        public int? TemplateId { get; set; }

        public int GuestWaitingCount { get; set; }
        public int GuestAttendanceCount { get; set; }
        public int TicketMailedCount { get; set; }

        [NotMapped]
        public int TicketRemainingCount
        {
            get
            {
                var remaining = Capacity - GuestAttendanceCount;
                if (remaining < 0) remaining = 0;
                return remaining;
            }


        }
        [NotMapped]
        public bool IsAtCapacity => TicketRemainingCount <= 0;

        public ICollection<Guest> Guests { get; set; }
        public virtual Template Template { get; set; }

        public void RegisterGuest(Guest guest)
        {
            guest.ResponseDate = DateTime.Now;
            if (TicketRemainingCount - guest.TicketCount < 0)
            {
                guest.IsAttending = false;
                guest.IsWaiting = true;
                guest.WaitingDate = DateTime.Now;
                GuestWaitingCount += guest.TicketCount ?? 0;
            }
            else
            {
                guest.IsAttending = true;
                guest.IsWaiting = false;
                guest.WaitingDate = null;
                GuestAttendanceCount += guest.TicketCount ?? 0;
            }

        }

        public void MailTicket(Guest guest)
        {
            guest.IsMailed = true;
            guest.MailedDate = DateTime.Now;

            TicketMailedCount += guest.TicketCount ?? 0;

        }

        public void AddTickets(Guest guest, int additionalTickets)
        {
            guest.TicketCount = guest.TicketCount + additionalTickets;
            GuestAttendanceCount = GuestAttendanceCount + additionalTickets;
        }

        public void MoveToMailQueue(Guest guest)
        {
            guest.IsWaiting = false;
            guest.IsAttending = true;
            GuestWaitingCount = GuestWaitingCount - guest.TicketCount ?? 0;
            GuestAttendanceCount += guest.TicketCount ?? 0;
        }
    }
}