using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using ZumZumFood.Domain.Abstracts;
using ZumZumFood.Domain.Entities;
using ZumZumFood.Persistence.Data;

namespace ZumZumFood.Persistence.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Category>> GetAllAsync(Expression<Func<Category, bool>> expression = null,
         Func<IQueryable<Category>, IIncludableQueryable<Category, object>>? include = null)
        {
            return await base.GetAllAsync(expression, include);
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await base.GetSingleAsync(x => x.CategoryId == id);
        }

        public async Task<bool> SaveOrUpdateAsync(Category category)
        {
            try
            {
                if (category.CategoryId == 0)
                {
                    await base.AddAsync(category);
                }
                else
                {
                    base.UpdateAsync(category);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Category category)
        {
            try
            {
                base.DeleteAsync(category);
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
