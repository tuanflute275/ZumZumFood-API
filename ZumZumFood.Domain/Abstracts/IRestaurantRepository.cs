namespace ZumZumFood.Domain.Abstracts
{
    public interface IRestaurantRepository
    {
        Task<IEnumerable<Restaurant>> GetAllAsync(Expression<Func<Restaurant, bool>> expression = null,
           Func<IQueryable<Restaurant>, IIncludableQueryable<Restaurant, object>>? include = null);
        Task<Restaurant?> GetByIdAsync(int id);
        Task<bool> SaveOrUpdateAsync(Restaurant restaurant);
        Task<bool> DeleteAsync(Restaurant restaurant);
    }
}
