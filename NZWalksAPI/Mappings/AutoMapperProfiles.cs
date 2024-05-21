using AutoMapper;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;

namespace NZWalksAPI.Mappings
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Region,RegionDto>().ReverseMap();
           
            CreateMap<AddRegionRequestDto, Region>().ReverseMap();

            CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();
            

        }
    }
}
