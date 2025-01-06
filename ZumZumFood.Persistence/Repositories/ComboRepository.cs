namespace ZumZumFood.Persistence.Repositories
{
    public class ComboRepository : BaseRepository<Combo>, IComboRepository
    {
        public ComboRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Combo>> GetAllAsync(Expression<Func<Combo, bool>> expression = null,
         Func<IQueryable<Combo>, IIncludableQueryable<Combo, object>>? include = null)
        {
            return await base.GetAllAsync(expression, include);
        }

        public async Task<Combo?> GetByIdAsync(int id)
        {
            return await base.GetSingleAsync(x => x.ComboId == id);
        }

        public async Task<bool> SaveOrUpdateAsync(Combo combo)
        {
            try
            {
                if (combo.ComboId == 0)
                {
                    await base.AddAsync(combo);
                }
                else
                {
                    base.UpdateAsync(combo);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Combo combo)
        {
            try
            {
                base.DeleteAsync(combo);
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
