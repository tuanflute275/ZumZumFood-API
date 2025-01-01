using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using ZumZumFood.Domain.Entities;

namespace ZumZumFood.Domain.Abstracts
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync(Expression<Func<Product, bool>> expression = null,
           Func<IQueryable<Product>, IIncludableQueryable<Product, object>>? include = null);
        Task<Product?> GetByIdAsync(int id);
        Task<bool> SaveOrUpdateAsync(Product product);
        Task<bool> DeleteAsync(Product product);
        Task<bool> DeleteRangeAsync(List<Product> listData);
    }
}
