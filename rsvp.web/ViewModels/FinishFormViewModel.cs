using DonorGateway.Domain;
using rsvp.web.Helpers;
using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;


namespace rsvp.web.ViewModels
{
    public class FinishFormViewModel
    {
        public int EventId { get; set; }

        public string CurrentDate { get; private set; }
        public byte[] EventTemplateImage { get; set; }
        public string EventTemplateMimeType { get; set; }

        public string EventTemplateHeaderText { get; set; }
        public string EventTemplateBodyText { get; set; }
        public string EventTemplateFooterText { get; set; }
        public string EventTemplateFAQText { get; set; }
        public string EventTemplateYesResponseText { get; set; }
        public string EventTemplateNoResponseText { get; set; }
        public string EventTemplateWaitingResponseText { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string InsideSalutation { get; set; }
        public string EmailSalutation { get; set; }
        public bool IsAttending { get; set; }
        public bool IsWaiting { get; set; }

        public string EventName { get; set; }
        public string EventDisplayName { get; set; }
        public string EventSpeaker { get; set; }
        public string EventVenue { get; set; }
        public string EventStreet { get; set; }
        public string EventCity { get; set; }
        public string EventState { get; set; }
        public string EventZipcode { get; set; }
        public string EventStartDate { get; set; }
        public string EventEndDate { get; set; }
        public string EventVenueOpenDate { get; set; }

        public Template Template { get; set; }

        public FinishFormViewModel()
        {
            CurrentDate = DateTime.Today.ToString("d");
        }

        public void ProcessMessages()
        {
            var properties = typeof(FinishFormViewModel).GetProperties().Where(p => p.PropertyType == typeof(string));
            foreach (var prop in properties)
            {
                ProcessField(prop);
            }
        }

        private void ProcessField(PropertyInfo field)
        {
            if (field.GetValue(this, null) == null) return;

            var text = field.GetValue(this, null).ToString();

            var propertyInfos = typeof(FinishFormViewModel).GetProperties().Where(p => p.PropertyType == typeof(string));

            foreach (var propertyInfo in propertyInfos)
            {
                if (propertyInfo.GetValue(this, null) == null) continue;
                var propValue = propertyInfo.GetValue(this, null).ToString();

                if (string.IsNullOrWhiteSpace(propValue)) continue;
                text = ReplaceText(text, propertyInfo.Name, propValue);
            }

            field.SetValue(this, Convert.ChangeType(text, field.PropertyType), null);

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