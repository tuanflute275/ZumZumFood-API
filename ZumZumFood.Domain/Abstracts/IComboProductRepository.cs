namespace ZumZumFood.Domain.Abstracts
{
    public interface IComboProductRepository
    {
        Task<IEnumerable<ComboProduct>> GetAllAsync(Expression<Func<ComboProduct, bool>> expression = null,
           Func<IQueryable<ComboProduct>, IIncludableQueryable<ComboProduct, object>>? include = null);
        Task<ComboProduct?> GetByIdAsync(int id);
        Task<bool> SaveOrUpdateAsync(ComboProduct comboProduct);
        Task<bool> DeleteAsync(ComboProduct comboProduct);
    }
}
