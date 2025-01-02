namespace ZumZumFood.Domain.Entities
{
    [Table("Carts")]
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("CartId")]
        public int CartId { get; set; }

        [Column("Quantity")]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        [Column("TotalAmount")]
        [Range(0, int.MaxValue)]
        public double? TotalAmount { get; set; }

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
