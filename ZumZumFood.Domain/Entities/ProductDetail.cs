namespace ZumZumFood.Domain.Entities
{
    [Table("ProductDetails")]
    public class ProductDetail : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ProductDetailId")]
        public int ProductDetailId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; } // Tên sản phẩm, topping, combo

        [Column(TypeName = "nvarchar(20)")]
        public string Type { get; set; } // Loại sản phẩm: "Topping"

        [Column(TypeName = "nvarchar(20)")]
        public string? Size { get; set; } // Kích thước món (nếu có), VD: "Nhỏ", "Vừa", "Lớn"

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Price { get; set; } // Giá của sản phẩm hoặc topping

        [Column]
        public int? Quantity { get; set; } // Số lượng còn trong kho

        [Column(TypeName = "nvarchar(500)")]
        public string? Description { get; set; } // Mô tả sản phẩm hoặc chi tiết thêm

        [Column]
        public bool IsAvailable { get; set; } = true; // Trạng thái còn hàng

        [Column("ProductId")]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
