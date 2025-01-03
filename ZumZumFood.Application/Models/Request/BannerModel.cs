namespace ZumZumFood.Application.Models.Request
{
    public class BannerModel
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? OldImage { get; set; }
        public bool? IsActive { get; set; } = true;
        public string BannerType { get; set; } = "APP"; // APP, Restaurant
        public string? RestaurantId { get; set; }
    }
}
