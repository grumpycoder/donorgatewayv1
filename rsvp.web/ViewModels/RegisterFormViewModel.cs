using DonorGateway.Domain;
using FluentValidation;
using FluentValidation.Attributes;
using System;

namespace rsvp.web.ViewModels
{
    [Validator(typeof(RegisterFormViewModelValidator))]
    public class RegisterFormViewModel
    {
        public int GuestId { get; set; }
        
        public string PromoCode { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Address2 { get; set; }

        public string Address3 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zipcode { get; set; }

        public string Comment { get; set; }

        public int? TicketCount { get; set; }

        public int? EventTicketAllowance { get; set; }

        public bool? IsAttending { get; set; }
        public bool? IsWaiting { get; set; }

        public DateTime? ResponseDate { get; set; }
        public DateTime? WaitingDate { get; set; }

        public int? EventId { get; set; }

        public string EventName { get; set; }

        public bool IsRegistered => ResponseDate != null;

        public Template Template { get; set; }

    }

    public class RegisterFormViewModelValidator : AbstractValidator<RegisterFormViewModel>
    {
        public RegisterFormViewModelValidator()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("Name is required.");
            RuleFor(x => x.Address).NotNull().WithMessage("Address is required.");
            RuleFor(x => x.City).NotNull().WithMessage("City is required.");
            RuleFor(x => x.State).NotNull().WithMessage("State is required.");
            RuleFor(x => x.Zipcode).NotNull().WithMessage("Zipcode is required.");
            RuleFor(x => x.Phone).NotNull().WithMessage("Phone is required.");
            RuleFor(x => x.Email).NotNull().WithMessage("Email is required.");
            RuleFor(x => x.IsAttending).NotNull().WithMessage("Please select Yes or No if you are attending.");
        }

    }
}