using AutoMapper;
using ZumZumFood.Application.Models.DTOs;
using ZumZumFood.Application.Models.RequestModel;
using ZumZumFood.Domain.Entities;

namespace ZumZumFood.Application.Configuration
{
    public class AutomapConfig : Profile
    {
        public AutomapConfig()
        {
            CreateMap<UserRequestModel, User>().ReverseMap();
            CreateMap<User, UserDTO>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => string.Join(", ", src.UserRoles.Select(ur => ur.Role.RoleName))));

        }
    }
}
