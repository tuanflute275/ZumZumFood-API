namespace ZumZumFood.Persistence.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Order>> GetAllAsync(Expression<Func<Order, bool>> expression = null,
         Func<IQueryable<Order>, IIncludableQueryable<Order, object>>? include = null)
        {
            return await base.GetAllAsync(expression, include);
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await base.GetSingleAsync(x => x.OrderId == id);
        }

        public async Task<bool> SaveOrUpdateAsync(Order order)
        {
            try
            {
                if (order.OrderId == 0)
                {
                    await base.AddAsync(order);
                }
                else
                {
                    base.UpdateAsync(order);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Order order)
        {
            try
            {
                base.DeleteAsync(order);
                await base.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteRangeAsync(List<Order> listData)
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
