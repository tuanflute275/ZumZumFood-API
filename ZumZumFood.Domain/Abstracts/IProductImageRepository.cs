namespace ZumZumFood.Domain.Abstracts
{
    public interface IProductImageRepository
    {
        Task<IEnumerable<ProductImage>> GetAllAsync(Expression<Func<ProductImage, bool>> expression = null,
           Func<IQueryable<ProductImage>, IIncludableQueryable<ProductImage, object>>? include = null);
        Task<ProductImage?> GetByIdAsync(int id);
        Task<bool> SaveOrUpdateAsync(ProductImage productImage);
        Task<bool> DeleteAsync(ProductImage productImage);
        Task<bool> DeleteRangeAsync(List<ProductImage> listData);
    }
}
