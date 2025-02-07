namespace ZumZumFood.Application.Abstracts
{
    public interface IProductService
    {
        Task<ResponseObject> GetAllPaginationAsync(ProductQuery productQuery);
        Task<ResponseObject> GetByIdAsync(int id);
        Task<ResponseObject> SaveAsync(ProductModel model);
        Task<ResponseObject> UpdateAsync(int id, ProductModel model);
        Task<ResponseObject> DeleteAsync(int id);
        Task<ResponseObject> DeleteFlagAsync(int id, string deleteBy);
        Task<ResponseObject> GetDeletedListAsync();
        Task<ResponseObject> RestoreAsync(int id);
    }
}
