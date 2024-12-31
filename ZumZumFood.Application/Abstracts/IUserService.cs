using ZumZumFood.Application.Models.RequestModel;
using ZumZumFood.Application.Models.Response;

namespace ZumZumFood.Application.Abstracts
{
    public interface IUserService
    {
        Task<ResponseObject> GetAllPaginationAsync(string? keyword, string? sort, int pageNo = 1);
        Task<ResponseObject> GetByIdAsync(int id);
        Task<ResponseObject> SaveAsync(UserRequestModel model);
        Task<ResponseObject> UpdateAsync(int id, UserRequestModel model);
        Task<ResponseObject> DeleteAsync(int id);
        Task<ResponseObject> DeleteFlagAsync(int id);
        Task<ResponseObject> GetDeletedListAsync();
        Task<ResponseObject> RestoreAsync(int id);
    }
}
