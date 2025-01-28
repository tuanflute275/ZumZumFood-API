namespace ZumZumFood.Application.Models.Request
{
    public class WishlistModel
    {
        [Required(ErrorMessage = "UserId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "UserId must be a positive integer.")]
        public int UserId { get; set; }

        public int? ProductId { get; set; }

        public int? ComboId { get; set; }
        public string? CreateBy { get; set; }
    }
}
