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
           .ForMember(dest => dest.DeleteDate, opt => opt.MapFrom(src =>
               src.DeleteDate.HasValue ? src.DeleteDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null))
           .ReverseMap()
           .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src =>
               !string.IsNullOrEmpty(src.CreateDate) ? DateTime.Parse(src.CreateDate) : (DateTime?)null))
           .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src =>
               !string.IsNullOrEmpty(src.UpdateDate) ? DateTime.Parse(src.UpdateDate) : (DateTime?)null))
           .ForMember(dest => dest.DeleteDate, opt => opt.MapFrom(src =>
               !string.IsNullOrEmpty(src.DeleteDate) ? DateTime.Parse(src.DeleteDate) : (DateTime?)null));

            CreateMap<Category, CategoryDTO>()
            .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src =>
               src.CreateDate.HasValue ? src.CreateDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null))
           .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src =>
               src.UpdateDate.HasValue ? src.UpdateDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null))
           .ForMember(dest => dest.DeleteDate, opt => opt.MapFrom(src =>
               src.DeleteDate.HasValue ? src.DeleteDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null))
           .ReverseMap()
           .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src =>
               !string.IsNullOrEmpty(src.CreateDate) ? DateTime.Parse(src.CreateDate) : (DateTime?)null))
           .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src =>
               !string.IsNullOrEmpty(src.UpdateDate) ? DateTime.Parse(src.UpdateDate) : (DateTime?)null))
           .ForMember(dest => dest.DeleteDate, opt => opt.MapFrom(src =>
               !string.IsNullOrEmpty(src.DeleteDate) ? DateTime.Parse(src.DeleteDate) : (DateTime?)null));

            CreateMap<Restaurant, RestaurantDTO>()
           .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src =>
               src.CreateDate.HasValue ? src.CreateDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null))
           .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src =>
               src.UpdateDate.HasValue ? src.UpdateDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null))
           .ForMember(dest => dest.DeleteDate, opt => opt.MapFrom(src =>
               src.DeleteDate.HasValue ? src.DeleteDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null))
           .ReverseMap()
           .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src =>
               !string.IsNullOrEmpty(src.CreateDate) ? DateTime.Parse(src.CreateDate) : (DateTime?)null))
           .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src =>
               !string.IsNullOrEmpty(src.UpdateDate) ? DateTime.Parse(src.UpdateDate) : (DateTime?)null))
           .ForMember(dest => dest.DeleteDate, opt => opt.MapFrom(src =>
               !string.IsNullOrEmpty(src.DeleteDate) ? DateTime.Parse(src.DeleteDate) : (DateTime?)null));

            CreateMap<Product, ProductDTO>()
           .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src =>
               src.CreateDate.HasValue ? src.CreateDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null))
           .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src =>
               src.UpdateDate.HasValue ? src.UpdateDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null))
           .ForMember(dest => dest.DeleteDate, opt => opt.MapFrom(src =>
               src.DeleteDate.HasValue ? src.DeleteDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : null))
           .ReverseMap()
           .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src =>
               !string.IsNullOrEmpty(src.CreateDate) ? DateTime.Parse(src.CreateDate) : (DateTime?)null))
           .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src =>
               !string.IsNullOrEmpty(src.UpdateDate) ? DateTime.Parse(src.UpdateDate) : (DateTime?)null))
           .ForMember(dest => dest.DeleteDate, opt => opt.MapFrom(src =>
               !string.IsNullOrEmpty(src.DeleteDate) ? DateTime.Parse(src.DeleteDate) : (DateTime?)null));
        }
    }
}
