namespace ZumZumFood.Domain.Abstracts
{
    public interface ILocationRepository
    {
        Task<IEnumerable<Location>> GetAllAsync(Expression<Func<Location, bool>> expression = null,
           Func<IQueryable<Location>, IIncludableQueryable<Location, object>>? include = null);
        Task<Location?> GetByIdAsync(int id);
        Task<bool> SaveOrUpdateAsync(Location location);
        Task<bool> DeleteAsync(Location location);
    }
}
