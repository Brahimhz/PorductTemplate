using AutoMapper;
using Product.Core.Models;
using Product.Identity.DTOs;

namespace Product.Identity.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterUser, User>()
                .ForMember(u => u.SecurityStamp, opt => opt.MapFrom(_ => Guid.NewGuid().ToString()));

            CreateMap<User, UserDto>();
        }
    }
}
