namespace ZumZumFood.Domain.Abstracts
{
    public interface ICodeRepository
    {
        Task<IEnumerable<Code>> GetAllAsync(Expression<Func<Code, bool>> expression = null,
           Func<IQueryable<Code>, IIncludableQueryable<Code, object>>? include = null);
        Task<Code?> GetByCodeIdAsync(string codeId);
        Task<bool> SaveAsync(Code code);
        Task<bool> UpdateAsync(Code code);
        Task<bool> DeleteAsync(Code code);
    }
}
