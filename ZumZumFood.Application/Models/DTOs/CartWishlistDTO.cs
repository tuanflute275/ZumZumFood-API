namespace ZumZumFood.Application.Models.DTOs
{
    public class CartWishlistDTO
    {
        public int WishlistId { get; set; }
        public UserMapperDTO Users { get; set; }
        public List<ProductDTO> Products { get; set; } = new List<ProductDTO>();
        public List<ComboDTO> Combos { get; set; } = new List<ComboDTO>();
    }
}
