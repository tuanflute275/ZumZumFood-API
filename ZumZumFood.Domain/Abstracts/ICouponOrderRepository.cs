namespace ZumZumFood.Domain.Abstracts
{
    public interface ICouponOrderRepository
    {
        Task<IEnumerable<CouponOrder>> GetAllAsync(Expression<Func<CouponOrder, bool>> expression = null,
           Func<IQueryable<CouponOrder>, IIncludableQueryable<CouponOrder, object>>? include = null);
        Task<CouponOrder?> GetByIdAsync(int id);
        Task<bool> SaveOrUpdateAsync(CouponOrder couponOrder);
        Task<bool> DeleteAsync(CouponOrder couponOrder);
    }
}
