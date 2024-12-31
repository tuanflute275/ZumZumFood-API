using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using ZumZumFood.Domain.Abstracts;
using ZumZumFood.Domain.Entities;
using ZumZumFood.Persistence.Data;

namespace ZumZumFood.Persistence.Repositories
{
    public class RestaurantRepository : BaseRepository<Restaurant>, IRestaurantRepository
    {
        public RestaurantRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Restaurant>> GetAllAsync(Expression<Func<Restaurant, bool>> expression = null,
         Func<IQueryable<Restaurant>, IIncludableQueryable<Restaurant, object>>? include = null)
        {
            return await base.GetAllAsync(expression, include);
        }

        public async Task<Restaurant?> GetByIdAsync(int id)
        {
            return await base.GetSingleAsync(x => x.RestaurantId == id);
        }

        public async Task<bool> SaveOrUpdateAsync(Restaurant restaurant)
        {
            try
            {
                if (restaurant.RestaurantId == 0)
                {
                    await base.AddAsync(restaurant);
                }
                else
                {
                    base.UpdateAsync(restaurant);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Restaurant restaurant)
        {
            try
            {
                base.DeleteAsync(restaurant);
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
