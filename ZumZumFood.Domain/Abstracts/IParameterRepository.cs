using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using ZumZumFood.Domain.Entities;

namespace ZumZumFood.Domain.Abstracts
{
    public interface IParameterRepository
    {
        Task<IEnumerable<Parameter>> GetAllAsync(Expression<Func<Parameter, bool>> expression = null,
           Func<IQueryable<Parameter>, IIncludableQueryable<Parameter, object>>? include = null);
        Task<Parameter?> GetByIdAsync(int id);
        Task<bool> SaveOrUpdateAsync(Parameter parameter);
        Task<bool> DeleteAsync(Parameter parameter);
    }
}
