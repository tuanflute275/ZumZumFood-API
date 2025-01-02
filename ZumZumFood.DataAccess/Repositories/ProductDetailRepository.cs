namespace ZumZumFood.Persistence.Repositories
{
    public class ProductDetailRepository : BaseRepository<ProductDetail>, IProductDetailRepository
    {
        public ProductDetailRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ProductDetail>> GetAllAsync(Expression<Func<ProductDetail, bool>> expression = null,
         Func<IQueryable<ProductDetail>, IIncludableQueryable<ProductDetail, object>>? include = null)
        {
            return await base.GetAllAsync(expression, include);
        }

        public async Task<ProductDetail?> GetByIdAsync(int id)
        {
            return await base.GetSingleAsync(x => x.ProductDetailId == id);
        }

        public async Task<bool> SaveOrUpdateAsync(ProductDetail productDetail)
        {
            try
            {
                if (productDetail.ProductDetailId == 0)
                {
                    await base.AddAsync(productDetail);
                }
                else
                {
                    base.UpdateAsync(productDetail);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(ProductDetail productDetail)
        {
            try
            {
                base.DeleteAsync(productDetail);
                await base.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteRangeAsync(List<ProductDetail> listData)
        {
            try
            {
                base.DeleteRangeAsync(listData);
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
