namespace ZumZumFood.Persistence.Repositories
{
    public class BrandRepository : BaseRepository<Brand>, IBrandRepository
    {
        public BrandRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Brand>> GetAllAsync(Expression<Func<Brand, bool>> expression = null,
         Func<IQueryable<Brand>, IIncludableQueryable<Brand, object>>? include = null)
        {
            return await base.GetAllAsync(expression, include);
        }

        public async Task<Brand?> GetByIdAsync(int id)
        {
            return await base.GetSingleAsync(x => x.BrandId == id);
        }

        public async Task<bool> SaveOrUpdateAsync(Brand restaurant)
        {
            try
            {
                if (restaurant.BrandId == 0)
                {
                    await base.AddAsync(restaurant);
                }
                else
                {
                    base.UpdateAsync(restaurant);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Brand restaurant)
        {
            try
            {
                base.DeleteAsync(restaurant);
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
