namespace ZumZumFood.Application.Models.Queries.Components
{
    public class CouponQuery : BaseQuery<Coupon>
    {
        public string Code { get; set; }
    }
}
