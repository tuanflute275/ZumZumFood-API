namespace ZumZumFood.Persistence.Repositories
{
    public class LocationRepository : BaseRepository<Location>, ILocationRepository
    {
        public LocationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Location>> GetAllAsync(Expression<Func<Location, bool>> expression = null,
         Func<IQueryable<Location>, IIncludableQueryable<Location, object>>? include = null)
        {
            return await base.GetAllAsync(expression, include);
        }

        public async Task<Location?> GetByIdAsync(int id)
        {
            return await base.GetSingleAsync(x => x.Id == id);
        }

        public async Task<bool> SaveOrUpdateAsync(Location location)
        {
            try
            {
                if (location.Id == 0)
                {
                    await base.AddAsync(location);
                }
                else
                {
                    base.UpdateAsync(location);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Location location)
        {
            try
            {
                base.DeleteAsync(location);
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
