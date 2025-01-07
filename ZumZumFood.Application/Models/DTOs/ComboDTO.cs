namespace ZumZumFood.Application.Models.DTOs
{
    public class ComboDTO : BaseDTO
    {
        public int ComboId { get; set; }
        public string Name { get; set; } 
        public string? Image { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; } 
        public bool IsActive { get; set; }
        
        public List<ProductDTO> Products { get; set; } = new List<ProductDTO>();
    }
}
