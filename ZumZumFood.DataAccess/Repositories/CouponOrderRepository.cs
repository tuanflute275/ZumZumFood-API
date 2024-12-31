using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using ZumZumFood.Domain.Abstracts;
using ZumZumFood.Domain.Entities;
using ZumZumFood.Persistence.Data;

namespace ZumZumFood.Persistence.Repositories
{
    public class CouponOrderRepository : BaseRepository<CouponOrder>, ICouponOrderRepository
    {
        public CouponOrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<CouponOrder>> GetAllAsync(Expression<Func<CouponOrder, bool>> expression = null,
         Func<IQueryable<CouponOrder>, IIncludableQueryable<CouponOrder, object>>? include = null)
        {
            return await base.GetAllAsync(expression, include);
        }

        public async Task<CouponOrder?> GetByIdAsync(int id)
        {
            return await base.GetSingleAsync(x => x.Id == id);
        }

        public async Task<bool> SaveOrUpdateAsync(CouponOrder couponOrder)
        {
            try
            {
                if (couponOrder.Id == 0)
                {
                    await base.AddAsync(couponOrder);
                }
                else
                {
                    base.UpdateAsync(couponOrder);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(CouponOrder couponOrder)
        {
            try
            {
                base.DeleteAsync(couponOrder);
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
