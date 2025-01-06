namespace ZumZumFood.Domain.Abstracts
{
    public interface IBrandRepository
    {
        Task<IEnumerable<Brand>> GetAllAsync(Expression<Func<Brand, bool>> expression = null,
           Func<IQueryable<Brand>, IIncludableQueryable<Brand, object>>? include = null);
        Task<Brand?> GetByIdAsync(int id);
        Task<bool> SaveOrUpdateAsync(Brand restaurant);
        Task<bool> DeleteAsync(Brand restaurant);
    }
}
