using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ZumZumFood.Domain.Entities
{
    [Table("couponConditions")]
    public class CouponCondition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CouponConditionId { get; set; }

        [Column]
        public int CouponId { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        [Required]
        public string Attribute { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Required]
        public string Operator { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        [Required]
        public string Value { get; set; }

        [Column(name: "discountAmount")]
        [Range(0, double.MaxValue)]
        [Required]
        public double DiscountAmount { get; set; }

        [ForeignKey("CouponId")]
        [JsonIgnore]
        public virtual Coupon Coupon { get; set; }
    }
}
