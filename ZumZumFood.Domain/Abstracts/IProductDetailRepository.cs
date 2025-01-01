using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using ZumZumFood.Domain.Entities;

namespace ZumZumFood.Domain.Abstracts
{
    public interface IProductDetailRepository
    {
        Task<IEnumerable<ProductDetail>> GetAllAsync(Expression<Func<ProductDetail, bool>> expression = null,
           Func<IQueryable<ProductDetail>, IIncludableQueryable<ProductDetail, object>>? include = null);
        Task<ProductDetail?> GetByIdAsync(int id);
        Task<bool> SaveOrUpdateAsync(ProductDetail productDetail);
        Task<bool> DeleteAsync(ProductDetail productDetail);
        Task<bool> DeleteRangeAsync(List<ProductDetail> listData);
    }
}
