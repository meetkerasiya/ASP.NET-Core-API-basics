using AutoMapper;

namespace CityInfo.API
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Entities.City, Model.CityWithoutPointsOfInterestDto>();
            CreateMap<Model.PointOfInterestForCreationDto, Entities.PointOfInterest>();
            CreateMap<Entities.PointOfInterest, Model.PointOfInterestDto>();
        }
    }
}
