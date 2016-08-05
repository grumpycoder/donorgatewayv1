using System;

namespace DonorGateway.Domain
{
    public class Guest : BaseEntity
    {
        public string LookupId { get; set; }
        public string FinderNumber { get; set; }
        public string ConstituentType { get; set; }
        public string SourceCode { get; set; }
        public string InteractionId { get; set; }

        public string MembershipYear { get; set; }
        public bool? LeadershipCouncil { get; set; }
        public string InsideSalutation { get; set; }
        public string OutsideSalutation { get; set; }
        public string HouseholdSalutation1 { get; set; }
        public string HouseholdSalutation2 { get; set; }
        public string HouseholdSalutation3 { get; set; }
        public string EmailSalutation { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string StateName { get; set; }
        public string Zipcode { get; set; }
        public string Country { get; set; }

        public int? TicketCount { get; set; }
        public bool IsMailed { get; set; } = false;

        public bool IsAttending { get; set; } = false;
        public bool IsWaiting { get; set; } = false;

        public DateTime? ResponseDate { get; set; }
        public DateTime? MailedDate { get; set; }
        public DateTime? WaitingDate { get; set; }

        public string MailedBy { get; set; }

        public int? EventId { get; set; }

        public virtual Event Event { get; set; }

        //ADDED FOR IMPORT/EXPORT FIELDS
        public string ActualDate { get; set; }
        public string ExpectedDate { get; set; }
        public string Comment { get; set; }
        public string ResponseType { get; set; }
        public string SPLCComment { get; set; }
        public string Status { get; set; }
        public string ContactMethod { get; set; }
        public string Summary { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Owner { get; set; }
    }
}