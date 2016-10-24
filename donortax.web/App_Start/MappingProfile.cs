using AutoMapper;
using DonorGateway.Domain;

namespace web.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Constituent, DemographicChange>().ReverseMap();
            });

        }
        protected override void Configure()
        {

        }
    }
}