using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using ZumZumFood.Domain.Abstracts;
using ZumZumFood.Domain.Entities;
using ZumZumFood.Persistence.Data;

namespace ZumZumFood.Persistence.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetAllAsync(Expression<Func<Product, bool>> expression = null,
         Func<IQueryable<Product>, IIncludableQueryable<Product, object>>? include = null)
        {
            return await base.GetAllAsync(expression, include);
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await base.GetSingleAsync(x => x.ProductId == id);
        }

        public async Task<bool> SaveOrUpdateAsync(Product product)
        {
            try
            {
                if (product.ProductId == 0)
                {
                    await base.AddAsync(product);
                }
                else
                {
                    base.UpdateAsync(product);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Product product)
        {
            try
            {
                base.DeleteAsync(product);
                await base.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteRangeAsync(List<Product> listData)
        {
            try
            {
                base.DeleteRangeAsync(listData);
                await base.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
