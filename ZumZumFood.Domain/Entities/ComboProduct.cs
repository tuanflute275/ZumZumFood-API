namespace ZumZumFood.Domain.Entities
{
    [Table("ComboProducts")]
    public class ComboProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ComboProductId { get; set; } // Khóa chính

        [Column]
        public int ComboId { get; set; } // Khóa ngoại đến bảng Combo
        [ForeignKey("ComboId")]
        public virtual Combo Combo { get; set; }

        [Column]
        public int ProductId { get; set; } // Khóa ngoại đến bảng Product
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
