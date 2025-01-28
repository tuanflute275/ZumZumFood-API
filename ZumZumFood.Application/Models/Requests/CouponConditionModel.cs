namespace ZumZumFood.Application.Models.Request
{
    public class CouponConditionModel
    {
        [Required(ErrorMessage = "CouponId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "CouponId must be a positive number.")]
        public int CouponId { get; set; }

        [Required(ErrorMessage = "Attribute is required.")]
        [StringLength(100, ErrorMessage = "Attribute cannot exceed 100 characters.")]
        public string Attribute { get; set; }

        [Required(ErrorMessage = "Operator is required.")]
        [StringLength(20, ErrorMessage = "Operator cannot exceed 20 characters.")]
        public string Operator { get; set; }

        [Required(ErrorMessage = "Value is required.")]
        [StringLength(200, ErrorMessage = "Value cannot exceed 200 characters.")]
        public string Value { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "DiscountAmount must be a non-negative number.")]
        public double DiscountAmount { get; set; }
    }
}
