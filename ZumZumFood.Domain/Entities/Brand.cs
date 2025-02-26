﻿namespace ZumZumFood.Domain.Entities
{
    [Table("Brands")]
    public class Brand : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BrandId { get; set; }

        [Required]
        [StringLength(150)]
        [Column(TypeName = "nvarchar(200)")]
        public string Name { get; set; }

        public string? Slug { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string? Image { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string? Address { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string? PhoneNumber { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string? Email { get; set; } 

        [Column(TypeName = "nvarchar(500)")]
        public string? Description { get; set; }

        [Column]
        public bool? IsActive { get; set; } = true;  // Trạng thái kích hoạt
        public TimeSpan? OpenTime { get; set; }  // Thời gian mở cửa
        public TimeSpan? CloseTime { get; set; }  // Thời gian đóng cửa

        // Quan hệ với bảng Product (Menu của nhà hàng)
        public virtual ICollection<Product> Products { get; set; }  // Các sản phẩm (món ăn) của nhà hàng
    }
}
