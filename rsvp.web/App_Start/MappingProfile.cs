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
                //cfg.CreateMap<Event, EventViewModel>()
                //    .ForMember(vm => vm.EventId, map => map.MapFrom(m => m.Id))
                //    .ForMember(vm => vm.EventName, map => map.MapFrom(m => m.Name))
                //    .ForMember(vm => vm.EventDisplayName, map => map.MapFrom(m => m.DisplayName))
                //    .ReverseMap();

                //cfg.CreateMap<Event, EventViewModel>()
                //   .ForMember(vm => vm.EventId, map => map.MapFrom(m => m.Id))
                //   .ForMember(vm => vm.EventName, map => map.MapFrom(m => m.Name))
                //   .ForMember(vm => vm.EventDisplayName, map => map.MapFrom(m => m.DisplayName))
                //   .ReverseMap();

                cfg.CreateMap<Event, EventViewModel>()
                    .ForMember(d => d.EventId, d => d.MapFrom(m => m.Id))
                    .ReverseMap();

                cfg.CreateMap<Guest, RegisterFormViewModel>()
                    .ForMember(vm => vm.GuestId, map => map.MapFrom(m => m.Id))
                    .ForMember(vm => vm.PromoCode, map => map.MapFrom(m => m.FinderNumber))
                    .ForMember(vm => vm.Template, map => map.MapFrom(m => m.Event.Template))
                    .ReverseMap();

                cfg.CreateMap<Guest, FinishFormViewModel>();
            });

        }
        protected override void Configure()
        {

        }
    }
}