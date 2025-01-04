namespace ZumZumFood.Persistence.Repositories
{
    public class LogRepository : BaseRepository<Log>, ILogRepository
    {
        public LogRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Log>> GetAllAsync(Expression<Func<Log, bool>> expression = null,
         Func<IQueryable<Log>, IIncludableQueryable<Log, object>>? include = null)
        {
            return await base.GetAllAsync(expression, include);
        }

        public async Task<Log?> GetByIdAsync(int id)
        {
            return await base.GetSingleAsync(x => x.LogId == id);
        }

        public async Task<bool> SaveOrUpdateAsync(Log log)
        {
            try
            {
                if (log.LogId == 0)
                {
                    await base.AddAsync(log);
                }
                else
                {
                    base.UpdateAsync(log);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Log log)
        {
            try
            {
                base.DeleteAsync(log);
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
