namespace ZumZumFood.Application.Models.DTOs
{
    public class CategoryMapperDTO
    {
        public int CategoryId { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string? Slug { get; set; }
        public bool? IsActive { get; set; }
        public string? Description { get; set; }
        public string? CreateBy { get; set; }
        public string? CreateDate { get; set; }
        public string? UpdateBy { get; set; }
        public string? UpdateDate { get; set; }
        public string? DeleteBy { get; set; }
        public string? DeleteDate { get; set; }
        public bool? DeleteFlag { get; set; }
        public List<ProductDTO> Products { get; set; } = new List<ProductDTO>();
    }
}
