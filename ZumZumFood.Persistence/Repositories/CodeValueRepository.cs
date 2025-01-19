namespace ZumZumFood.Persistence.Repositories
{
    public class CodeValueRepository : BaseRepository<CodeValues>, ICodeValueRepository
    {
        public CodeValueRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<CodeValues>> GetAllAsync(Expression<Func<CodeValues, bool>> expression = null,
         Func<IQueryable<CodeValues>, IIncludableQueryable<CodeValues, object>>? include = null)
        {
            return await base.GetAllAsync(expression, include);
        }

        public async Task<CodeValues?> GetByCodeIdAndCodeValueAsync(string codeId, string codeValue)
        {
            return await base.GetSingleAsync(x => x.CodeId == codeId && x.CodeValue == codeValue);
        }

        public async Task<bool> SaveAsync(CodeValues codeValues)
        {
            try
            {
                await base.AddAsync(codeValues);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(CodeValues codeValues)
        {
            try
            {
                base.UpdateAsync(codeValues);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(CodeValues codeValues)
        {
            try
            {
                base.DeleteAsync(codeValues);
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
