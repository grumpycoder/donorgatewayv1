using DonorGateway.Domain;

namespace admin.web.ViewModels
{
    public class GuestSearchViewModel : Pager<Guest>
    {
        public string LookupId { get; set; }
        public string FinderNumber { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int? EventId { get; set; }
        public bool? IsMailed { get; set; }
        public bool? IsWaiting { get; set; }
        public bool? IsAttending { get; set; }

        public int? GuestCount { get; set; }
    }
}
