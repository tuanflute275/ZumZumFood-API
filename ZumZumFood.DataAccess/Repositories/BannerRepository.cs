using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using ZumZumFood.Domain.Abstracts;
using ZumZumFood.Domain.Entities;
using ZumZumFood.Persistence.Data;

namespace ZumZumFood.Persistence.Repositories
{
    public class BannerRepository : BaseRepository<Banner>, IBannerRepository
    {
        public BannerRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Banner>> GetAllAsync(Expression<Func<Banner, bool>> expression = null,
         Func<IQueryable<Banner>, IIncludableQueryable<Banner, object>>? include = null)
        {
            return await base.GetAllAsync(expression, include);
        }

        public async Task<Banner?> GetByIdAsync(int id)
        {
            return await base.GetSingleAsync(x => x.BannerId == id);
        }

        public async Task<bool> SaveOrUpdateAsync(Banner banner)
        {
            try
            {
                if (banner.BannerId == 0)
                {
                    await base.AddAsync(banner);
                }
                else
                {
                    base.UpdateAsync(banner);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Banner banner)
        {
            try
            {
                base.DeleteAsync(banner);
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
