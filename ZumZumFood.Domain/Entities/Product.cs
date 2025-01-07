namespace ZumZumFood.Domain.Entities
{
    [Table("Products")]
    public class Product : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Required]
        [StringLength(150)]
        [Column(TypeName = "nvarchar(200)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string? Slug { get; set; }

        [StringLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        public string? Image { get; set; }

        [Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public double Price { get; set; }

        [Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public double? Discount { get; set; } = 0;

        [Column]
        public bool? IsActive { get; set; } = true;  // Trạng thái kích hoạt

        [Column(TypeName = "nvarchar(max)")]
        public string? Description { get; set; }

        [Column]
        public bool IsBestSeller { get; set; } = false;  // Có phải là sản phẩm bán chạy (true/false)

        [Column]
        public bool IsFeatured { get; set; } = false;    // Có phải là sản phẩm nổi bật (true/false)

        [Column]
        public bool IsNew { get; set; } = true;         // Có phải là sản phẩm mới (true/false)

        [Column]
        public int ReviewsCount { get; set; } = 0;

        [Range(0, 5)]
        [Column(TypeName = "decimal(3,2)")] // Lưu điểm số từ 0.00 đến 5.00
        public decimal? Rating { get; set; } = 0;

        [Column]
        public int OrderCount { get; set; } = 0; //Số lần đặt hàng

        [Column]
        [Range(0, 1440)]
        public int AverageDeliveryTime { get; set; } = 20; // Mặc định là 20 phút

        [Column]
        public int BrandId { get; set; }
        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; }

        [Column]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
        public virtual ICollection<ProductComment> ProductComments { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
    }
}
