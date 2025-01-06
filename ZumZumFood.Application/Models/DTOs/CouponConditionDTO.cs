namespace ZumZumFood.Application.Models.DTOs
{
    public class CouponConditionDTO
    {
        public int CouponConditionId { get; set; }
        public int CouponId { get; set; }
        public string Attribute { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
        public double DiscountAmount { get; set; }
    }
}
