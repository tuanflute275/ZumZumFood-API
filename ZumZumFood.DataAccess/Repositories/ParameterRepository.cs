using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using ZumZumFood.Domain.Abstracts;
using ZumZumFood.Domain.Entities;
using ZumZumFood.Persistence.Data;

namespace ZumZumFood.Persistence.Repositories
{
    public class ParameterRepository : BaseRepository<Parameter>, IParameterRepository
    {
        public ParameterRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Parameter>> GetAllAsync(Expression<Func<Parameter, bool>> expression = null,
         Func<IQueryable<Parameter>, IIncludableQueryable<Parameter, object>>? include = null)
        {
            return await base.GetAllAsync(expression, include);
        }

        public async Task<Parameter?> GetByIdAsync(int id)
        {
            return await base.GetSingleAsync(x => x.ParameterId == id);
        }

        public async Task<bool> SaveOrUpdateAsync(Parameter parameter)
        {
            try
            {
                if (parameter.ParameterId == 0)
                {
                    await base.AddAsync(parameter);
                }
                else
                {
                    base.UpdateAsync(parameter);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Parameter parameter)
        {
            try
            {
                base.DeleteAsync(parameter);
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
