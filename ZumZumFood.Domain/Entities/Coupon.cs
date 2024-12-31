using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ZumZumFood.Domain.Entities
{
    [Table("Coupons")]
    public class Coupon
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CouponId { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        [Required]
        public string Code { get; set; }

        [Range(0, 100)]
        public int? Percent { get; set; }

        [Column(TypeName = "ntext")]
        public string? Description { get; set; }

        [Column]
        public bool IsActive { get; set; } = true;  // Trạng thái kích hoạt 
        public virtual ICollection<CouponOrder> CouponOrders { get; set; } = new List<CouponOrder>();
        public virtual ICollection<CouponCondition> CouponConditions { get; set; } = new List<CouponCondition>();
    }
}
