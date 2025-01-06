namespace ZumZumFood.Application.Models.DTOs
{
    public class ProductMapperDTO : BaseDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string? Slug { get; set; }
        public string? Image { get; set; }
        public double Price { get; set; }
        public double? Discount { get; set; }
        public bool? IsActive { get; set; }
        public string? Description { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<ProductImageDTO> ProductImages { get; set; } = new List<ProductImageDTO>();
        public List<ProductCommentDTO> ProductComments { get; set; } = new List<ProductCommentDTO>();
        public List<ProductDetailDTO> ProductDetails { get; set; } = new List<ProductDetailDTO>();
    }
}
