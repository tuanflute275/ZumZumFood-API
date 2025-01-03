namespace ZumZumFood.Application.Models.DTOs
{
    public class BannerDTO : BaseDTO
    {
        public int BannerId { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public bool? IsActive { get; set; } = true;
        public string BannerType { get; set; } // APP, Restaurant
        public int? RestaurantId { get; set; }
    }
}
