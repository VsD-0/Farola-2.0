using AutoMapper;
using Farola.Application.DTOs.Users;
using Farola.Domain.Entities;

namespace Farola.Application.Common.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : null));
        }
    }
}
