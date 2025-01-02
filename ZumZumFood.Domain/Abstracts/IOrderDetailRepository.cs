namespace ZumZumFood.Domain.Abstracts
{
    public interface IOrderDetailRepository
    {
        Task<IEnumerable<OrderDetail>> GetAllAsync(Expression<Func<OrderDetail, bool>> expression = null,
           Func<IQueryable<OrderDetail>, IIncludableQueryable<OrderDetail, object>>? include = null);
        Task<OrderDetail?> GetByIdAsync(int id);
        Task<bool> SaveOrUpdateAsync(OrderDetail orderDetail);
        Task<bool> DeleteAsync(OrderDetail orderDetail);
        Task<bool> DeleteRangeAsync(List<OrderDetail> listData);
    }
}
