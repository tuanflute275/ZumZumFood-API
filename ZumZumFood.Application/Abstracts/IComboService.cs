namespace ZumZumFood.Application.Abstracts
{
    public interface IComboService
    {
        Task<ResponseObject> GetAllPaginationAsync(ComboQuery comboQuery);
        Task<ResponseObject> GetByIdAsync(int id);
        Task<ResponseObject> SaveAsync(ComboModel model);
        Task<ResponseObject> UpdateAsync(int id, ComboModel model);
        Task<ResponseObject> DeleteAsync(int id);
        Task<ResponseObject> DeleteFlagAsync(int id, string deleteBy);
        Task<ResponseObject> GetDeletedListAsync();
        Task<ResponseObject> RestoreAsync(int id);
    }
}
