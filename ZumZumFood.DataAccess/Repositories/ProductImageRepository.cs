using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using ZumZumFood.Domain.Abstracts;
using ZumZumFood.Domain.Entities;
using ZumZumFood.Persistence.Data;

namespace ZumZumFood.Persistence.Repositories
{
    public class ProductImageRepository : BaseRepository<ProductImage>, IProductImageRepository
    {
        public ProductImageRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ProductImage>> GetAllAsync(Expression<Func<ProductImage, bool>> expression = null,
         Func<IQueryable<ProductImage>, IIncludableQueryable<ProductImage, object>>? include = null)
        {
            return await base.GetAllAsync(expression, include);
        }

        public async Task<ProductImage?> GetByIdAsync(int id)
        {
            return await base.GetSingleAsync(x => x.ProductImageId == id);
        }

        public async Task<bool> SaveOrUpdateAsync(ProductImage productImage)
        {
            try
            {
                if (productImage.ProductImageId == 0)
                {
                    await base.AddAsync(productImage);
                }
                else
                {
                    base.UpdateAsync(productImage);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(ProductImage productImage)
        {
            try
            {
                base.DeleteAsync(productImage);
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
