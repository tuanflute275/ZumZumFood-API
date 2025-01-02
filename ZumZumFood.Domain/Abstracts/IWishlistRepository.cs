namespace ZumZumFood.Domain.Abstracts
{
    public interface IWishlistRepository
    {
        Task<IEnumerable<Wishlist>> GetAllAsync(Expression<Func<Wishlist, bool>> expression = null,
           Func<IQueryable<Wishlist>, IIncludableQueryable<Wishlist, object>>? include = null);
        Task<Wishlist?> GetByIdAsync(int id);
        Task<bool> SaveOrUpdateAsync(Wishlist wishlist);
        Task<bool> DeleteAsync(Wishlist wishlist);
        Task<bool> DeleteRangeAsync(List<Wishlist> listData);
    }
}
