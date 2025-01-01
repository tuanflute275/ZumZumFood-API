namespace ZumZumFood.Application.Models.DTOs
{
    public class CategoryDTO : BaseDTO
    {
        public int CategoryId { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string? Slug { get; set; }
        public bool? IsActive { get; set; }
        public string? Description { get; set; }
    }
}
