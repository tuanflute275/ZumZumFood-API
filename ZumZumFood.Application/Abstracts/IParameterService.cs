namespace ZumZumFood.Application.Abstracts
{
    public interface IParameterService
    {
        Task<ResponseObject> GetAllPaginationAsync(ParameterQuery parameterQuery);
        Task<ResponseObject> GetByIdAsync(int id);
        Task<ResponseObject> SaveAsync(ParameterModel model);
        Task<ResponseObject> UpdateAsync(int id, ParameterModel model);
        Task<ResponseObject> DeleteAsync(int id);
        Task<ResponseObject> DeleteFlagAsync(int id, string deleteBy);
        Task<ResponseObject> GetDeletedListAsync();
        Task<ResponseObject> RestoreAsync(int id);
    }
}
