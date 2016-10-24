using CsvHelper.Configuration;
using DonorGateway.Domain;

namespace admin.web.ViewModels
{
    public class MailerMap : CsvClassMap<Mailer>
    {
        public MailerMap()
        {
            Map(m => m.FinderNumber).Name("Finder Number");
            Map(m => m.SourceCode).Name("Source Code");
            Map(m => m.FirstName).Name("First Name");
            Map(m => m.MiddleName).Name("Middle Name");
            Map(m => m.LastName).Name("Last Name");
            Map(m => m.Address).Name("Address Line 1");
            Map(m => m.Address2).Name("Address Line 2");
            Map(m => m.Address3).Name("Address Line 3");
            Map(m => m.City).Name("City");
            Map(m => m.State).Name("State Abbreviation");
            Map(m => m.ZipCode).Name("Zip Code");
        }

    }
}