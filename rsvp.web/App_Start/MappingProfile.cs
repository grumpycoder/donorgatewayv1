using AutoMapper;
using DonorGateway.Domain;
using rsvp.web.ViewModels;
// ReSharper disable CheckNamespace

namespace web.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            Mapper.Initialize(cfg =>
            {

                cfg.CreateMap<Guest, DemographicChange>()
                    .ForMember(d => d.Street, map => map.MapFrom(m => m.Address))
                    .ForMember(d => d.Street2, map => map.MapFrom(m => m.Address2))
                    .ReverseMap();

                cfg.CreateMap<Event, EventViewModel>()
                    .ForMember(d => d.EventId, d => d.MapFrom(m => m.Id))
                    .ReverseMap();


                cfg.CreateMap<RegisterFormViewModel, Guest>()
                    .ForMember(d => d.FinderNumber, opt => opt.MapFrom(o => o.PromoCode))
                    .ForMember(d => d.Id, opt => opt.MapFrom(o => o.GuestId));

                cfg.CreateMap<Guest, RegisterFormViewModel>()
                    .ForMember(vm => vm.GuestId, map => map.MapFrom(m => m.Id))
                    .ForMember(vm => vm.PromoCode, map => map.MapFrom(m => m.FinderNumber))
                    .ForMember(vm => vm.Template, map => map.MapFrom(m => m.Event.Template));


                cfg.CreateMap<Guest, FinishFormViewModel>();
            });

        }
        protected override void Configure()
        {

        }
    }
}