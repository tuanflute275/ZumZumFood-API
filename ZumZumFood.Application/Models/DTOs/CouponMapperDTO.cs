namespace ZumZumFood.Application.Models.DTOs
{
    public class CouponMapperDTO : BaseDTO
    {
        public int CouponId { get; set; }
        public string Code { get; set; }
        public int Percent { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public List<CouponConditionDTO> Conditions { get; set; } = new List<CouponConditionDTO>();
    }
}
