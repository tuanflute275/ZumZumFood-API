namespace ZumZumFood.Domain.Abstracts
{
    public interface ICartRepository
    {
        Task<IEnumerable<Cart>> GetAllAsync(Expression<Func<Cart, bool>> expression = null,
           Func<IQueryable<Cart>, IIncludableQueryable<Cart, object>>? include = null);
        Task<Cart?> GetByIdAsync(int id);
        Task<bool> SaveOrUpdateAsync(Cart cart);
        Task<bool> DeleteAsync(Cart cart);
        Task<bool> DeleteRangeAsync(List<Cart> listData);
    }
}
