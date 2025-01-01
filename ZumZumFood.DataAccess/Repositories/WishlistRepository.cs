using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using ZumZumFood.Domain.Abstracts;
using ZumZumFood.Domain.Entities;
using ZumZumFood.Persistence.Data;

namespace ZumZumFood.Persistence.Repositories
{
    public class WishlistRepository : BaseRepository<Wishlist>, IWishlistRepository
    {
        public WishlistRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Wishlist>> GetAllAsync(Expression<Func<Wishlist, bool>> expression = null,
         Func<IQueryable<Wishlist>, IIncludableQueryable<Wishlist, object>>? include = null)
        {
            return await base.GetAllAsync(expression, include);
        }

        public async Task<Wishlist?> GetByIdAsync(int id)
        {
            return await base.GetSingleAsync(x => x.WishlistId == id);
        }

        public async Task<bool> SaveOrUpdateAsync(Wishlist wishlist)
        {
            try
            {
                if (wishlist.WishlistId == 0)
                {
                    await base.AddAsync(wishlist);
                }
                else
                {
                    base.UpdateAsync(wishlist);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Wishlist wishlist)
        {
            try
            {
                base.DeleteAsync(wishlist);
                await base.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteRangeAsync(List<Wishlist> listData)
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
