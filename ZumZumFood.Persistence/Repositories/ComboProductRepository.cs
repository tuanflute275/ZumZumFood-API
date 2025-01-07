namespace ZumZumFood.Persistence.Repositories
{
    public class ComboProductRepository : BaseRepository<ComboProduct>, IComboProductRepository
    {
        public ComboProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ComboProduct>> GetAllAsync(Expression<Func<ComboProduct, bool>> expression = null,
         Func<IQueryable<ComboProduct>, IIncludableQueryable<ComboProduct, object>>? include = null)
        {
            return await base.GetAllAsync(expression, include);
        }

        public async Task<ComboProduct?> GetByIdAsync(int id)
        {
            return await base.GetSingleAsync(x => x.ComboProductId == id);
        }

        public async Task<bool> SaveOrUpdateAsync(ComboProduct comboProduct)
        {
            try
            {
                if (comboProduct.ComboProductId == 0)
                {
                    await base.AddAsync(comboProduct);
                }
                else
                {
                    base.UpdateAsync(comboProduct);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(ComboProduct comboProduct)
        {
            try
            {
                base.DeleteAsync(comboProduct);
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
