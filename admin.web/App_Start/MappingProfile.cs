using admin.web.ViewModels;
using AutoMapper;
using DonorGateway.Domain;
using web.ViewModels;

namespace web.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ApplicationUser, UserViewModel>().ReverseMap();

                cfg.CreateMap<Event, EventViewModel>().ReverseMap();

                cfg.CreateMap<Guest, GuestViewModel>().ReverseMap();

                cfg.CreateMap<Guest, DemographicChange>().ReverseMap();

                cfg.CreateMap<Constituent, DemographicChange>().ReverseMap();
                cfg.CreateMap<DemographicChange, DemographicViewModel>()
                    .ForMember(dest => dest.Source, map => map.MapFrom(m => m.Source.ToString()))
                    .ReverseMap();

                cfg.CreateMap<Guest, GuestExportViewModel>()
                    .ForMember(dest => dest.EventCode, opt => opt.MapFrom(src => src.Event.EventCode))
                    .ForMember(dest => dest.EventName, opt => opt.MapFrom(src => src.Event.Name));

                cfg.CreateMap<Constituent, ConstituentViewModel>().ReverseMap();

                cfg.CreateMap<Mailer, MailerViewModel>().ReverseMap();

                cfg.CreateMap<Guest, DemographicChange>()
                    .ForMember(d => d.Street, map => map.MapFrom(m => m.Address))
                    .ForMember(d => d.Street2, map => map.MapFrom(m => m.Address2))
                    .ReverseMap();




            });

        }
        protected override void Configure()
        {

        }
    }
}