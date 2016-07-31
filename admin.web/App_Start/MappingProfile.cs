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
            });

        }
        protected override void Configure()
        {

        }
    }
}