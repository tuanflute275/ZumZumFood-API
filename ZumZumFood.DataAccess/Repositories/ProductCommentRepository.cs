using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using ZumZumFood.Domain.Abstracts;
using ZumZumFood.Domain.Entities;
using ZumZumFood.Persistence.Data;

namespace ZumZumFood.Persistence.Repositories
{
    public class ProductCommentRepository : BaseRepository<ProductComment>, IProductCommentRepository
    {
        public ProductCommentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ProductComment>> GetAllAsync(Expression<Func<ProductComment, bool>> expression = null,
         Func<IQueryable<ProductComment>, IIncludableQueryable<ProductComment, object>>? include = null)
        {
            return await base.GetAllAsync(expression, include);
        }

        public async Task<ProductComment?> GetByIdAsync(int id)
        {
            return await base.GetSingleAsync(x => x.ProductCommentId == id);
        }

        public async Task<bool> SaveOrUpdateAsync(ProductComment productComment)
        {
            try
            {
                if (productComment.ProductCommentId == 0)
                {
                    await base.AddAsync(productComment);
                }
                else
                {
                    base.UpdateAsync(productComment);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(ProductComment productComment)
        {
            try
            {
                base.DeleteAsync(productComment);
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
