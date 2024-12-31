using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using ZumZumFood.Domain.Abstracts;
using ZumZumFood.Domain.Entities;
using ZumZumFood.Persistence.Data;

namespace ZumZumFood.Persistence.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Role>> GetAllAsync(Expression<Func<Role, bool>> expression = null,
         Func<IQueryable<Role>, IIncludableQueryable<Role, object>>? include = null)
        {
            return await base.GetAllAsync(expression, include);
        }

        public async Task<Role?> GetByIdAsync(int id)
        {
            return await base.GetSingleAsync(x => x.RoleId == id);
        }

        public async Task<bool> SaveOrUpdateAsync(Role role)
        {
            try
            {
                if (role.RoleId == 0)
                {
                    await base.AddAsync(role);
                }
                else
                {
                    base.UpdateAsync(role);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Role role)
        {
            try
            {
                base.DeleteAsync(role);
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
