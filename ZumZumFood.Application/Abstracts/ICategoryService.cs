namespace ZumZumFood.Application.Abstracts
{
    public interface ICategoryService
    {
        Task<ResponseObject> GetAllPaginationAsync(CategoryQuery categoryQuery);
        Task<ResponseObject> GetByIdAsync(int id);
        Task<ResponseObject> SaveAsync(CategoryModel model);
        Task<ResponseObject> UpdateAsync(int id, CategoryModel model);
        Task<ResponseObject> DeleteAsync(int id);
        Task<ResponseObject> DeleteFlagAsync(int id, string deleteBy);
        Task<ResponseObject> GetDeletedListAsync();
        Task<ResponseObject> RestoreAsync(int id);
    }
}
