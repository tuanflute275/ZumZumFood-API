 namespace ZumZumFood.Domain.Entities
{
    [Table("Coupons")]
    public class Coupon : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CouponId { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        [Required]
        public string Code { get; set; }

        [Column(TypeName = "ntext")]
        public string? Description { get; set; }

        [Column]
        public bool IsActive { get; set; } = true;  // Trạng thái kích hoạt 

        [Column(TypeName = "nvarchar(50)")]
        [Required]
        public string Scope { get; set; } = "System"; // Brand || Store // Giá trị mặc định là toàn hệ thống

        [Column]
        public int? ScopeId { get; set; } // ID của thương hiệu (brandId) nếu không phải toàn hệ thống

        public virtual ICollection<CouponCondition> CouponConditions { get; set; } = new List<CouponCondition>();
    }
}
