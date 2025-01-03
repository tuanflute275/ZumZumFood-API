namespace ZumZumFood.Application.Abstracts
{
    public interface IProductImageService
    {
        Task<ResponseObject> GetByIdAsync(int id);
        Task<ResponseObject> SaveAsync(ProductImageModel model);
        Task<ResponseObject> UpdateAsync(int id, ProductImageUpdateModel model);
        Task<ResponseObject> DeleteAsync(int id);
    }
}
