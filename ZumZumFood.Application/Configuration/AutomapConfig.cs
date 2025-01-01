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
            .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src =>
                src.CreateDate.HasValue ? src.CreateDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null))
            .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src =>
                src.UpdateDate.HasValue ? src.UpdateDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null))
            .ReverseMap()
            .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src =>
                !string.IsNullOrEmpty(src.CreateDate) ? DateTime.Parse(src.CreateDate) : (DateTime?)null))
            .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src =>
                !string.IsNullOrEmpty(src.UpdateDate) ? DateTime.Parse(src.UpdateDate) : (DateTime?)null));

            CreateMap<Category, CategoryDTO>()
            .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src =>
                src.CreateDate.HasValue ? src.CreateDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null))
            .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src =>
                src.UpdateDate.HasValue ? src.UpdateDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null))
            .ReverseMap()
            .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src =>
                !string.IsNullOrEmpty(src.CreateDate) ? DateTime.Parse(src.CreateDate) : (DateTime?)null))
            .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src =>
                !string.IsNullOrEmpty(src.UpdateDate) ? DateTime.Parse(src.UpdateDate) : (DateTime?)null));
        }
    }
}
