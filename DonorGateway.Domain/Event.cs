using DonorGateway.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
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

        public void CancelRegistration(Guest guest)
        {
            if(guest.IsWaiting ?? false) TicketMailedCount -= guest.TicketCount ?? 0;
            guest.ResponseDate = DateTime.Now;
            guest.IsAttending = false;
            guest.IsMailed = false;
            guest.IsWaiting = false;
            guest.WaitingDate = null;
            GuestAttendanceCount -= guest.TicketCount ?? 0;
            guest.TicketCount = 0;

        }

        public void RegisterGuest(Guest guest)
        {
            guest.ResponseDate = DateTime.Now;
            guest.TicketCount = guest.TicketCount ?? 0;
            guest.IsAttending = guest.IsAttending ?? false;

            var actualTickets = (TicketAllowance ?? 0) - (guest.TicketCount ?? 0);

            //Reset Attendance from reserved ticket allowance amount
            GuestAttendanceCount -= TicketAllowance ?? 0; 

            if (TicketRemainingCount - guest.TicketCount < 0)
            {
                guest.IsWaiting = true;
                guest.WaitingDate = DateTime.Now;
                GuestWaitingCount += actualTickets;
            }
            else
            {
                guest.IsWaiting = false;
                guest.WaitingDate = null;
                GuestAttendanceCount += actualTickets;
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
            if (guest.IsMailed) TicketMailedCount += additionalTickets;
            GuestAttendanceCount += additionalTickets;
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
            if (string.IsNullOrWhiteSpace(guest.Email)) return;

            ParseTemplate();
            ParseTemplate(guest);

            var message = Template.HeaderText + Template.BodyText;

            if (guest.IsWaiting == true && guest.IsAttending == true)
            {
                message += Template.WaitingResponseText;
            }
            if (guest.IsAttending == false)
            {
                message += Template.NoResponseText;
            }
            if (guest.IsAttending == true && guest.IsWaiting == false)
            {
                message += Template.YesResponseText;
            }

            message += "Sincerely, <br />";
            message += $"<p><img style='width:150px;' src=\"http://donate.splcenter.org/image/morris_dees_sig2.png\" /></p>";
            message += "Morris Dees<br />Founder, Southern Poverty Law Center";

            var html = AlternateView.CreateAlternateViewFromString(message, null, "text/html");

            var sendToAddress = new MailAddress(guest.Email);
            var sendFromAddress = new MailAddress(ConfigurationManager.AppSettings["SendFromAddress"], ConfigurationManager.AppSettings["SendFromDisplay"]);
            var subject = $"SPLC Event {DisplayName} Confirmation";

            var env = ConfigurationManager.AppSettings["Environment"];
            switch (env)
            {
                case "Prod":
                    break;
                default:
                    sendToAddress = new MailAddress(ConfigurationManager.AppSettings["SendToOverride"]);
                    break;
            }
            var mail = new MailMessage(sendFromAddress, sendToAddress)
            {
                Subject = subject,
                Body = message
            };

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

        public void ReserveTickets()
        {
            GuestAttendanceCount += TicketAllowance ?? 0;
        }
    }


}