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
                    .ForMember(dest => dest.EventName, opt => opt.MapFrom(src => src.Event.Name))
                    .ForMember(dest => dest.IsMailed, opt => opt.MapFrom(src => src.IsMailed ? "Yes" : "No"))
                    .ForMember(dest => dest.LeadershipCouncil, opt => opt.MapFrom(src => src.LeadershipCouncil.Value ? "No" : src.LeadershipCouncil.Value ? "Yes" : "No"))
                    .ForMember(dest => dest.Attending, opt => opt.MapFrom(src => src.IsWaiting.Value ? "Waiting" : src.IsAttending == null ? "" : src.IsAttending.Value ? "Yes" : "No"));

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

    public class CustomBoolResolver : ValueResolver<bool?, string>
    {
        #region Overrides of ValueResolver<bool?,string>

        protected override string ResolveCore(bool? source)
        {
            return "Yes";
            //return source != null ? source == true ? "Yes" : "No" : "No"; 
        }

        #endregion
    }
}