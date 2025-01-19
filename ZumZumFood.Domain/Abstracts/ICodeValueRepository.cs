namespace ZumZumFood.Domain.Abstracts
{
    public interface ICodeValueRepository
    {
        Task<IEnumerable<CodeValues>> GetAllAsync(Expression<Func<CodeValues, bool>> expression = null,
           Func<IQueryable<CodeValues>, IIncludableQueryable<CodeValues, object>>? include = null);
        Task<CodeValues?> GetByCodeIdAndCodeValueAsync(string codeId, string codeValue);
        Task<bool> SaveAsync(CodeValues codeValues);
        Task<bool> UpdateAsync(CodeValues codeValues);
        Task<bool> DeleteAsync(CodeValues codeValues);
    }
}
