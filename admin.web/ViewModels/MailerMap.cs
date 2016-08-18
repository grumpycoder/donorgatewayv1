using CsvHelper.Configuration;
using DonorGateway.Domain;

namespace admin.web.ViewModels
{
    public class MailerMap : CsvClassMap<Mailer>
    {
        public MailerMap()
        {
            Map(m => m.FinderNumber).Name("FinderNumber");
            Map(m => m.SourceCode).Name("SourceCode");
        }

    }
}