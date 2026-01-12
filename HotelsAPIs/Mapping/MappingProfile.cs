using AutoMapper;
using DataAccess;
using DeveloperChallenge.ViewModels;

namespace HotelsAPIs.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<Room, RoomViewModel>()
                .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel.Name))
                .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => Enum.GetName(typeof(RoomType), src.RoomTypeId)));

            CreateMap<Hotel, HotelViewModel>()
                .ForMember(dest => dest.Rooms, opt => opt.MapFrom(src => src.Rooms));
        }
    }
}
