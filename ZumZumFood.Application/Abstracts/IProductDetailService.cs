namespace ZumZumFood.Application.Abstracts
{
    public interface IProductDetailService
    {
        Task<ResponseObject> GetByIdAsync(int id);
        Task<ResponseObject> SaveAsync(ProductDetailModel model);
        Task<ResponseObject> UpdateAsync(int id, ProductDetailModel model);
        Task<ResponseObject> DeleteAsync(int id);
    }
}
