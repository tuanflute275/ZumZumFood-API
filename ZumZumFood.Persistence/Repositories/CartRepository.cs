namespace ZumZumFood.Persistence.Repositories
{
    public class CartRepository : BaseRepository<Cart>, ICartRepository
    {
        public CartRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Cart>> GetAllAsync(Expression<Func<Cart, bool>> expression = null,
         Func<IQueryable<Cart>, IIncludableQueryable<Cart, object>>? include = null)
        {
            return await base.GetAllAsync(expression, include);
        }

        public async Task<Cart?> GetByIdAsync(int id)
        {
            return await base.GetSingleAsync(x => x.CartId == id);
        }

        public async Task<bool> SaveOrUpdateAsync(Cart cart)
        {
            try
            {
                if (cart.CartId == 0)
                {
                    await base.AddAsync(cart);
                }
                else
                {
                    base.UpdateAsync(cart);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Cart cart)
        {
            try
            {
                base.DeleteAsync(cart);
                await base.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteRangeAsync(List<Cart> listData)
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
