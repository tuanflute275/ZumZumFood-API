namespace ZumZumFood.Domain.Abstracts
{
    public interface ILogRepository
    {
        Task<IEnumerable<Log>> GetAllAsync(Expression<Func<Log, bool>> expression = null,
           Func<IQueryable<Log>, IIncludableQueryable<Log, object>>? include = null);
        Task<Log?> GetByIdAsync(int id);
        Task<bool> SaveOrUpdateAsync(Log log);
        Task<bool> DeleteAsync(Log log);
    }
}
