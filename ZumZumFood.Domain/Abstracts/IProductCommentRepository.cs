namespace ZumZumFood.Domain.Abstracts
{
    public interface IProductCommentRepository
    {
        Task<IEnumerable<ProductComment>> GetAllAsync(Expression<Func<ProductComment, bool>> expression = null,
           Func<IQueryable<ProductComment>, IIncludableQueryable<ProductComment, object>>? include = null);
        Task<ProductComment?> GetByIdAsync(int id);
        Task<bool> SaveOrUpdateAsync(ProductComment productComment);
        Task<bool> DeleteAsync(ProductComment productComment);
        Task<bool> DeleteRangeAsync(List<ProductComment> listData);
    }
}
