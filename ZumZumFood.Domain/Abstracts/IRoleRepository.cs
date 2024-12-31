using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using ZumZumFood.Domain.Entities;

namespace ZumZumFood.Domain.Abstracts
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllAsync(Expression<Func<Role, bool>> expression = null,
           Func<IQueryable<Role>, IIncludableQueryable<Role, object>>? include = null);
        Task<Role?> GetByIdAsync(int id);
        Task<bool> SaveOrUpdateAsync(Role role);
        Task<bool> DeleteAsync(Role role);
    }
}
