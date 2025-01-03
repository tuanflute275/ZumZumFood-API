namespace ZumZumFood.Application.Configuration
{
    public class AutomapConfig : Profile
    {
        public AutomapConfig()
        {
            CreateMap<UserModel, User>().ReverseMap();
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
                .ForMember(dest => dest.OpenTime, opt => opt.MapFrom(src =>
                   src.OpenTime.HasValue ? src.OpenTime.Value.ToString(@"hh\:mm") : null))
               .ForMember(dest => dest.CloseTime, opt => opt.MapFrom(src =>
                   src.CloseTime.HasValue ? src.CloseTime.Value.ToString(@"hh\:mm") : null))
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

            CreateMap<Parameter, ParameterDTO>()
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
            CreateMap<ParameterModel, Parameter>().ReverseMap();

           CreateMap<Banner, BannerDTO>()
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

            CreateMap<Log, LogDTO>()
             .ForMember(dest => dest.TimeActionRequest, opt => opt.MapFrom(src =>
             src.TimeActionRequest.HasValue ? src.TimeActionRequest.Value.ToString("dd-MM-yyyy HH:mm:ss") : null))
             .ForMember(dest => dest.TimeLogin, opt => opt.MapFrom(src =>
                 src.TimeLogin.HasValue ? src.TimeLogin.Value.ToString("dd-MM-yyyy HH:mm:ss") : null))
             .ForMember(dest => dest.TimeLogout, opt => opt.MapFrom(src =>
                 src.TimeLogout.HasValue ? src.TimeLogout.Value.ToString("dd-MM-yyyy HH:mm:ss") : null))
             .ReverseMap()
             .ForMember(dest => dest.TimeActionRequest, opt => opt.MapFrom(src =>
                 !string.IsNullOrEmpty(src.TimeActionRequest) ? DateTime.Parse(src.TimeActionRequest) : (DateTime?)null))
             .ForMember(dest => dest.TimeLogin, opt => opt.MapFrom(src =>
                 !string.IsNullOrEmpty(src.TimeLogin) ? DateTime.Parse(src.TimeLogin) : (DateTime?)null))
             .ForMember(dest => dest.TimeLogout, opt => opt.MapFrom(src =>
                 !string.IsNullOrEmpty(src.TimeLogout) ? DateTime.Parse(src.TimeLogout) : (DateTime?)null));
        }
    }
}
