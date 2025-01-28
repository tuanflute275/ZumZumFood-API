namespace ZumZumFood.Application.Models.Request
{
    public class CartModel
    {
        [Required(ErrorMessage = "UserId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "UserId must be a positive integer.")]
        public int UserId { get; set; }

        public int? ProductId { get; set; }

        public int? ComboId { get; set; }

        public int Quantity { get; set; } = 1;
    }
}
