namespace ZumZumFood.Persistence.Repositories
{
    public class CodeRepository : BaseRepository<Code>, ICodeRepository
    {
        public CodeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Code>> GetAllAsync(Expression<Func<Code, bool>> expression = null,
         Func<IQueryable<Code>, IIncludableQueryable<Code, object>>? include = null)
        {
            return await base.GetAllAsync(expression, include);
        }

        public async Task<Code?> GetByCodeIdAsync(string codeId)
        {
            return await base.GetSingleAsync(x => x.CodeId == codeId);
        }

        public async Task<bool> SaveAsync(Code code)
        {
            try
            {
                await base.AddAsync(code);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Code code)
        {
            try
            {
                base.UpdateAsync(code);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Code code)
        {
            try
            {
                base.DeleteAsync(code);
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
