using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using ZumZumFood.Domain.Abstracts;
using ZumZumFood.Domain.Entities;
using ZumZumFood.Persistence.Data;

namespace ZumZumFood.Persistence.Repositories
{
    public class UserRoleRepository : BaseRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<UserRole>> GetAllAsync(Expression<Func<UserRole, bool>> expression = null,
         Func<IQueryable<UserRole>, IIncludableQueryable<UserRole, object>>? include = null)
        {
            return await base.GetAllAsync(expression, include);
        }

        public async Task<UserRole?> GetByIdAsync(int id)
        {
            return await base.GetSingleAsync(x => x.RoleId == id);
        }

        public async Task<UserRole?> GetUserByIdAsync(int userId)
        {
            return await base.GetSingleAsync(x => x.UserId == userId);
        }

        public async Task<bool> SaveOrUpdateAsync(UserRole userRole)
        {
            try
            {
                if (userRole.UserRoleId == 0)
                {
                    await base.AddAsync(userRole);
                }
                else
                {
                    base.UpdateAsync(userRole);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(UserRole userRole)
        {
            try
            {
                base.DeleteAsync(userRole);
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
