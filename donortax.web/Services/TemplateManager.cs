using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using DonorGateway.Data;
using DonorGateway.Domain;

namespace donortax.web.Services
{
    public class TemplateManager
    {
        public TemplateManager()
        {
            ValidationErrors = new List<KeyValuePair<string, string>>();
        }

        public List<KeyValuePair<string, string>> ValidationErrors { get; set; }

        public Template Get(string templateName)
        {
            using (var db = new DataContext())
            {
                return db.Templates.FirstOrDefault(t => t.Name == templateName);
            }
        }

        public bool Update(Template template)
        {
            bool ret = false;

            ret = Validate(template);

            if (ret)
            {
                // TODO: Create UPDATE code here
                using (var db = new DataContext())
                {
                    db.Templates.AddOrUpdate(template);
                    db.SaveChanges();
                }
            }

            return ret;
        }

        private bool Validate(Template template)
        {
            ValidationErrors.Clear();

            if (!string.IsNullOrEmpty(template.HeaderText))
            {
                if (template.HeaderText.ToLower() ==
                    template.HeaderText)
                {
                    ValidationErrors.Add(new
                        KeyValuePair<string, string>("Header Text",
                            "Header must not be all lower case."));
                }
            }

            return (ValidationErrors.Count == 0);
        }
    }
}