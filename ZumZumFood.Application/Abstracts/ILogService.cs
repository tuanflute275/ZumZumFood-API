namespace ZumZumFood.Application.Abstracts
{
    public interface ILogService
    {
        Task<ResponseObject> GetAllPaginationAsync(string? keyword, string? sort, int pageNo = 1);
        Task<ResponseObject> GetAllUserLoginPaginationAsync(string? keyword, string? sort, int pageNo = 1);
        Task<ResponseObject> GetByIdAsync(int id);
        Task<ResponseObject> SaveAsync(LogRequestModel model);
        Task<ResponseObject> UpdateAsync(int id);
        Task<ResponseObject> DeleteAsync(int id);
    }
}
