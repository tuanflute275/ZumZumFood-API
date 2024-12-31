using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using ZumZumFood.Domain.Entities;

namespace ZumZumFood.Domain.Abstracts
{
    public interface ICouponRepository
    {
        Task<IEnumerable<Coupon>> GetAllAsync(Expression<Func<Coupon, bool>> expression = null,
           Func<IQueryable<Coupon>, IIncludableQueryable<Coupon, object>>? include = null);
        Task<Coupon?> GetByIdAsync(int id);
        Task<bool> SaveOrUpdateAsync(Coupon coupon);
        Task<bool> DeleteAsync(Coupon coupon);
    }
}
