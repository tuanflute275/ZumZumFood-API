using ZumZumFood.Application.Models.Queries.Components;

namespace ZumZumFood.Application.Abstracts
{
    public interface IBrandService
    {
        Task<ResponseObject> GetAllPaginationAsync(BrandQuery brandQuery);
        Task<ResponseObject> GetByIdAsync(int id);
        Task<ResponseObject> SaveAsync(BrandModel model);
        Task<ResponseObject> UpdateAsync(int id, BrandModel model);
        Task<ResponseObject> DeleteAsync(int id);
        Task<ResponseObject> DeleteFlagAsync(int id, string deleteBy);
        Task<ResponseObject> GetDeletedListAsync();
        Task<ResponseObject> RestoreAsync(int id);
    }
}
