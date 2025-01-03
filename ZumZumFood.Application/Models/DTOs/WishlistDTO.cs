namespace ZumZumFood.Application.Models.DTOs
{
    public class WishlistDTO
    {
        public int WishlistId { get; set; }
        public ProductDTO Products { get; set; }
        public UserMapperDTO Users { get; set; }
    }
}
