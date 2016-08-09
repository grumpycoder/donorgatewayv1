using DonorGateway.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;

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
        public bool IsCancelled { get; set; }
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
            guest.IsAttending = guest.IsAttending;
            if (TicketRemainingCount - guest.TicketCount < 0)
            {
                guest.IsWaiting = true;
                guest.WaitingDate = DateTime.Now;
                GuestWaitingCount += guest.TicketCount ?? 0;
            }
            else
            {
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

        public void SendEmail(Guest guest)
        {
            ParseTemplate();
            ParseTemplate(guest);

            var message = Template.HeaderText + Template.BodyText;

            if (guest.IsWaiting && guest.IsAttending)
            {
                message += Template.WaitingResponseText;
            }
            if (!guest.IsAttending)
            {
                message += Template.NoResponseText;
            }
            if (guest.IsAttending && !guest.IsWaiting)
            {
                message += Template.YesResponseText;
            }

            //TODO: Possible error missing template image. 
            var imageData = Template.Image;
            var contentId = Guid.NewGuid().ToString();
            var linkedResource = new LinkedResource(new MemoryStream(imageData), "image/jpeg")
            {
                ContentId = contentId,
                TransferEncoding = TransferEncoding.Base64
            };
            message = $"<img style='width:100%;' src=\"cid:{contentId}\" />" + message;
            var html = AlternateView.CreateAlternateViewFromString(message, null, "text/html");
            html.LinkedResources.Add(linkedResource);


            //TODO: Replace from with web.config setting
            var mail = new MailMessage("rsvp@mail.com", "robbinfuqua@gmail.com");
            mail.AlternateViews.Add(html);
            var client = new SmtpClient();
            client.Send(mail);


        }

        public void ParseTemplate(Guest guest)
        {
            var properties = typeof(Template).GetProperties().Where(p => p.PropertyType == typeof(string));

            foreach (var prop in properties)
            {
                if (prop.GetValue(this.Template, null) == null) continue;

                var propValue = guest.Parse(prop.GetValue(this.Template, null).ToString());
                if (string.IsNullOrWhiteSpace(propValue)) continue;

                prop.SetValue(this.Template, Convert.ChangeType(propValue, prop.PropertyType), null);

            }

        }

        public void ParseTemplate()
        {
            var properties = typeof(Template).GetProperties().Where(p => p.PropertyType == typeof(string));

            foreach (var prop in properties)
            {
                if (prop.GetValue(this.Template, null) == null) continue;

                var propValue = Parse(prop.GetValue(this.Template, null).ToString());
                if (string.IsNullOrWhiteSpace(propValue)) continue;

                prop.SetValue(this.Template, Convert.ChangeType(propValue, prop.PropertyType), null);

            }

        }

        private string Parse(string message)
        {
            var properties = typeof(Event).GetProperties().Where(p => p.PropertyType == typeof(DateTime?) || p.PropertyType == typeof(string));
            foreach (var prop in properties)
            {
                if (prop.GetValue(this, null) == null) continue;

                var propValue = prop.GetValue(this, null).ToString();
                if (prop.PropertyType == typeof(DateTime?))
                {
                    propValue = Convert.ToDateTime(prop.GetValue(this, null)).ToString("dddd, MMMM d, yyyy @ h:mm tt");
                }

                if (string.IsNullOrWhiteSpace(propValue)) continue;
                message = ReplaceText(message, prop.Name, propValue);
            }
            return message;
        }

        private static string ReplaceText(string stringToReplace, string fieldName, string fieldValue)
        {
            var pattern = "@{" + fieldName + "}";

            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            var matches = regex.Matches(stringToReplace);

            return matches.Replace(stringToReplace, fieldValue);

        }

    }


}