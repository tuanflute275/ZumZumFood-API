using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using ZumZumFood.Domain.Entities;

namespace ZumZumFood.Domain.Abstracts
{
    public interface IUserRoleRepository
    {
        Task<IEnumerable<UserRole>> GetAllAsync(Expression<Func<UserRole, bool>> expression = null,
           Func<IQueryable<UserRole>, IIncludableQueryable<UserRole, object>>? include = null);
        Task<UserRole?> GetByIdAsync(int id);
        Task<UserRole?> GetUserByIdAsync(int userId);
        Task<bool> SaveOrUpdateAsync(UserRole userRole);
        Task<bool> DeleteAsync(UserRole userRole);
    }
}
