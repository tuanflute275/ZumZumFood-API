using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using ZumZumFood.Domain.Abstracts;
using ZumZumFood.Domain.Entities;
using ZumZumFood.Persistence.Data;

namespace ZumZumFood.Persistence.Repositories
{
    public class CouponRepository : BaseRepository<Coupon>, ICouponRepository
    {
        public CouponRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Coupon>> GetAllAsync(Expression<Func<Coupon, bool>> expression = null,
         Func<IQueryable<Coupon>, IIncludableQueryable<Coupon, object>>? include = null)
        {
            return await base.GetAllAsync(expression, include);
        }

        public async Task<Coupon?> GetByIdAsync(int id)
        {
            return await base.GetSingleAsync(x => x.CouponId == id);
        }

        public async Task<bool> SaveOrUpdateAsync(Coupon coupon)
        {
            try
            {
                if (coupon.CouponId == 0)
                {
                    await base.AddAsync(coupon);
                }
                else
                {
                    base.UpdateAsync(coupon);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Coupon coupon)
        {
            try
            {
                base.DeleteAsync(coupon);
                await base.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
