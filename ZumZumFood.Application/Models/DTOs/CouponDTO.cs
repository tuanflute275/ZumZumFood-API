namespace ZumZumFood.Application.Models.DTOs
{
    public class CouponDTO : BaseDTO
    {
        public int CouponId { get; set; }
        public string Code { get; set; }
        public int Percent { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
