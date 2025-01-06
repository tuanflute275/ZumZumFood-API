namespace ZumZumFood.Domain.Entities
{
    [Table("Combos")]
    public class Combo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ComboId { get; set; }

        [Required]
        [StringLength(150)]
        [Column(TypeName = "nvarchar(200)")]
        public string Name { get; set; } // Tên combo

        [Column(TypeName = "nvarchar(100)")]
        public string? Image { get; set; } // Hình ảnh của combo (nếu có)

        [Column(TypeName = "nvarchar(max)")]
        public string? Description { get; set; } // Mô tả chi tiết combo

        [Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public double Price { get; set; } // Giá của combo

        [Column]
        public bool IsActive { get; set; } = true;  // Trạng thái combo có hoạt động hay không

        // Quan hệ với bảng Product
        public virtual ICollection<Product> Products { get; set; } = new List<Product>(); // Combo có nhiều món ăn
    }
}
