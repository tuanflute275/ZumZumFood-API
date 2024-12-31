using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using ZumZumFood.Domain.Entities;

namespace ZumZumFood.Domain.Abstracts
{
    public interface ICouponConditionRepository
    {
        Task<IEnumerable<CouponCondition>> GetAllAsync(Expression<Func<CouponCondition, bool>> expression = null,
           Func<IQueryable<CouponCondition>, IIncludableQueryable<CouponCondition, object>>? include = null);
        Task<CouponCondition?> GetByIdAsync(int id);
        Task<bool> SaveOrUpdateAsync(CouponCondition couponCondition);
        Task<bool> DeleteAsync(CouponCondition couponCondition);
    }
}
