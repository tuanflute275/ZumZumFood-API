namespace ZumZumFood.Persistence.Repositories
{
    public class CouponConditionRepository : BaseRepository<CouponCondition>, ICouponConditionRepository
    {
        public CouponConditionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<CouponCondition>> GetAllAsync(Expression<Func<CouponCondition, bool>> expression = null,
         Func<IQueryable<CouponCondition>, IIncludableQueryable<CouponCondition, object>>? include = null)
        {
            return await base.GetAllAsync(expression, include);
        }

        public async Task<CouponCondition?> GetByIdAsync(int id)
        {
            return await base.GetSingleAsync(x => x.CouponConditionId == id);
        }

        public async Task<bool> SaveOrUpdateAsync(CouponCondition couponCondition)
        {
            try
            {
                if (couponCondition.CouponConditionId == 0)
                {
                    await base.AddAsync(couponCondition);
                }
                else
                {
                    base.UpdateAsync(couponCondition);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(CouponCondition couponCondition)
        {
            try
            {
                base.DeleteAsync(couponCondition);
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
