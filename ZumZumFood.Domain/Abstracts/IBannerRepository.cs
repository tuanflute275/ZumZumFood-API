using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using ZumZumFood.Domain.Entities;

namespace ZumZumFood.Domain.Abstracts
{
    public interface IBannerRepository
    {
        Task<IEnumerable<Banner>> GetAllAsync(Expression<Func<Banner, bool>> expression = null,
           Func<IQueryable<Banner>, IIncludableQueryable<Banner, object>>? include = null);
        Task<Banner?> GetByIdAsync(int id);
        Task<bool> SaveOrUpdateAsync(Banner banner);
        Task<bool> DeleteAsync(Banner banner);
    }
}
