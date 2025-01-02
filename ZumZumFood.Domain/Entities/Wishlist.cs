namespace ZumZumFood.Domain.Entities
{
    [Table("Wishlists")]
    public class Wishlist
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WishlistId { get; set; }

        [Required]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        [Column("ProductId")]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [Column("UserId")]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
