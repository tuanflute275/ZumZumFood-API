namespace ZumZumFood.Domain.Abstracts
{
    public interface IComboRepository
    {
        Task<IEnumerable<Combo>> GetAllAsync(Expression<Func<Combo, bool>> expression = null,
           Func<IQueryable<Combo>, IIncludableQueryable<Combo, object>>? include = null);
        Task<Combo?> GetByIdAsync(int id);
        Task<bool> SaveOrUpdateAsync(Combo combo);
        Task<bool> DeleteAsync(Combo combo);
    }
}
