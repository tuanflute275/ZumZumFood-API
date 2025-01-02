namespace ZumZumFood.Domain.Abstracts
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync(Expression<Func<Order, bool>> expression = null,
           Func<IQueryable<Order>, IIncludableQueryable<Order, object>>? include = null);
        Task<Order?> GetByIdAsync(int id);
        Task<bool> SaveOrUpdateAsync(Order order);
        Task<bool> DeleteAsync(Order order);
        Task<bool> DeleteRangeAsync(List<Order> listData);
    }
}
