namespace ZumZumFood.Application.Models.DTOs
{
    public class ProductDTO : BaseDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string? Slug { get; set; }
        public string? Image { get; set; }
        public double Price { get; set; }
        public double? Discount { get; set; }
        public bool? IsActive { get; set; }
        public string? Description { get; set; }
        public int RestaurantId { get; set; }
        public int RestaurantName { get; set; }
        public int CategoryId { get; set; }
        public int CategoryName { get; set; }
    }
}
