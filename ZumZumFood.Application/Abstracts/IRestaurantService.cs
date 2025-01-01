using ZumZumFood.Application.Models.Request;
using ZumZumFood.Application.Models.Response;

namespace ZumZumFood.Application.Abstracts
{
    public interface IRestaurantService
    {
        Task<ResponseObject> GetAllPaginationAsync(string? keyword, string? sort, int pageNo = 1);
        Task<ResponseObject> GetByIdAsync(int id);
        Task<ResponseObject> SaveAsync(RestaurantRequestModel model);
        Task<ResponseObject> UpdateAsync(int id, RestaurantRequestModel model);
        Task<ResponseObject> DeleteAsync(int id);
        Task<ResponseObject> DeleteFlagAsync(int id);
        Task<ResponseObject> GetDeletedListAsync();
        Task<ResponseObject> RestoreAsync(int id);
    }
}
