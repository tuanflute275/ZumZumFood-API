namespace ZumZumFood.Persistence.Repositories
{
    public class OrderDetailRepository : BaseRepository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<OrderDetail>> GetAllAsync(Expression<Func<OrderDetail, bool>> expression = null,
         Func<IQueryable<OrderDetail>, IIncludableQueryable<OrderDetail, object>>? include = null)
        {
            return await base.GetAllAsync(expression, include);
        }

        public async Task<OrderDetail?> GetByIdAsync(int id)
        {
            return await base.GetSingleAsync(x => x.OrderDetailId == id);
        }

        public async Task<bool> SaveOrUpdateAsync(OrderDetail orderDetail)
        {
            try
            {
                if (orderDetail.OrderDetailId == 0)
                {
                    await base.AddAsync(orderDetail);
                }
                else
                {
                    base.UpdateAsync(orderDetail);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(OrderDetail orderDetail)
        {
            try
            {
                base.DeleteAsync(orderDetail);
                await base.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteRangeAsync(List<OrderDetail> listData)
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
