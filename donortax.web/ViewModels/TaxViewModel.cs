using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Migrations;
using System.Linq;
using AutoMapper;
using donortax.web.Services;
using DonorGateway.Data;
using DonorGateway.Domain;

namespace donortax.web.ViewModels
{
    public class TaxViewModel
    {

        public TaxViewModel()
        {
            Entity = new Constituent();
            SearchEntity = new Constituent();
            TaxItems = new List<TaxItem>();
            EventCommand = "Search";
            IsDetailsVisible = false;
            SelectedTaxYear = DateTime.Now.Year - 1;
            AcceptTerms = false;
        }

        private const string templateName = "DonorTax";

        public Constituent SearchEntity { get; set; }
        public Constituent Entity { get; set; }
        public List<TaxItem> TaxItems { get; set; }
        public Template Template { get; set; }
        public List<KeyValuePair<string, string>> ValidationErrors { get; set; }

        public int SelectedTaxYear { get; set; }

        [Display(Name = "I have read and accept the policy")]
        public bool AcceptTerms { get; set; }

        public decimal TotalTax { get; set; }

        public string EventCommand { get; set; }
        public bool IsDetailsVisible { get; set; }
        public bool IsEditVisible { get; set; }

        public bool IsTaxDataAvailable { get; set; }
        public bool IsValid { get; set; }


        public void HandleRequest()
        {
            switch (EventCommand.ToLower())
            {
                case "list":
                case "search":
                    Search();
                    GetTemplate();
                    break;
                case "edit":
                    Edit();
                    break;
                case "save":
                    Save();
                    break;
            }
        }

        private void GetTemplate()
        {
            var mgr = new TemplateManager();
            Template = mgr.Get(templateName);
        }

        private void Search()
        {
            var mgr = new TaxManager();
            Entity = mgr.Search(SearchEntity);
            ValidationErrors = mgr.ValidationErrors;

            if (!AcceptTerms)
                ValidationErrors.Add(new KeyValuePair<string, string>("Accept", "You must accept the policy."));

            if (ValidationErrors.Count > 0)
            {
                IsValid = false;
            }

            if (Entity == null) IsValid = false;

            if (!IsValid) return;

            TotalTax = Entity.TaxItems.Where(t => t.TaxYear == SelectedTaxYear).Sum(x => x.Amount);
            TaxItems = Entity.TaxItems.Where(t => t.TaxYear == SelectedTaxYear).OrderBy(x => x.DonationDate).ToList();
            IsTaxDataAvailable = TaxItems.Count > 0;

            if (string.IsNullOrEmpty(Entity.Email))
            {
                EditMode();
            }
            else
            {
                IsDetailsVisible = true;
            }
        }

        private void EditMode()
        {
            IsEditVisible = true;
            IsDetailsVisible = false;
        }

        private void Edit()
        {
            Search();
            EditMode();
        }

        private void Save()
        {
            IsValid = true;
            var mgr = new TaxManager();
            mgr.Update(Entity);
            ValidationErrors = mgr.ValidationErrors;

            if (ValidationErrors.Count > 0)
            {
                IsValid = false;
            }
            if (!IsValid)
            {
                IsEditVisible = true;
                IsDetailsVisible = false;
            }
            else
            {
                IsEditVisible = false;
                IsDetailsVisible = true;
                Search();
                GetTemplate();
            }
        }

    }


    public class TaxManager
    {
        public TaxManager()
        {
            ValidationErrors = new List<KeyValuePair<string, string>>();
        }

        public List<KeyValuePair<string, string>> ValidationErrors { get; set; }

        public Constituent Search(Constituent entity)
        {
            using (var db = new DataContext())
            {
                var ret = db.Constituents.Include("TaxItems").FirstOrDefault(c => c.FinderNumber == entity.FinderNumber);
                ValidateSearch(entity);
                if (ret == null) ValidationErrors.Add(new KeyValuePair<string, string>("Not Found", "No tax records found for given supporter."));

                return ret;
            }
        }

        private bool ValidateSearch(Constituent entity)
        {
            ValidationErrors.Clear();

            if (entity.FinderNumber == null) ValidationErrors.Add(new KeyValuePair<string, string>("Supporter Id", "Supporter Id is required."));

            return ValidationErrors.Count == 0;
        }

        private bool Validate(Constituent entity)
        {
            ValidationErrors.Clear();
            if (string.IsNullOrWhiteSpace(entity.Name)) ValidationErrors.Add(new KeyValuePair<string, string>("Name", "Name is required"));
            if (string.IsNullOrWhiteSpace(entity.Email)) ValidationErrors.Add(new KeyValuePair<string, string>("Email", "Email is required"));

            return ValidationErrors.Count == 0;
        }

        public void Update(Constituent entity)
        {
            var isValid = Validate(entity);
            if (!isValid) return;

            using (var db = new DataContext())
            {
                var existing = db.Constituents.Find(entity.Id);
                if (entity.Equals(existing)) return;

                var demoChange = Mapper.Map<DemographicChange>(entity);
                demoChange.Source = Source.Tax;

                demoChange.UpdatedBy = "donor";
                demoChange.Source = Source.Tax;

                db.DemographicChanges.Add(demoChange);
                db.Constituents.AddOrUpdate(entity);
                db.SaveChanges();
            }
        }
    }

}