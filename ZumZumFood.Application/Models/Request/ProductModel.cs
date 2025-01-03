namespace ZumZumFood.Application.Models.Request
{
    public class ProductModel
    {
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, ErrorMessage = "Product name can't be longer than 100 characters.")]
        public string Name { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? OldImage { get; set; } = "";

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public double Price { get; set; }


        [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100.")]
        public double? Discount { get; set; } = 0;
        public bool? IsActive { get; set; } = true;
        public string? Description { get; set; } = "";

        [Required(ErrorMessage = "Restaurant ID is required.")]
        public int RestaurantId { get; set; }

        [Required(ErrorMessage = "Category ID is required.")]
        public int CategoryId { get; set; }
    }
}
