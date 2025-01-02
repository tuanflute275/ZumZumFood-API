namespace ZumZumFood.Domain.Abstracts
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync(Expression<Func<Category, bool>> expression = null,
           Func<IQueryable<Category>, IIncludableQueryable<Category, object>>? include = null);
        Task<Category?> GetByIdAsync(int id);
        Task<bool> SaveOrUpdateAsync(Category category);
        Task<bool> DeleteAsync(Category category);
    }
}
