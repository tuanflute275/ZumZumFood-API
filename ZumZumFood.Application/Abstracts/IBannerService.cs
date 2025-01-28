namespace ZumZumFood.Application.Abstracts
{
    public interface IBannerService
    {
        Task<ResponseObject> GetAllPaginationAsync(BannerQuery bannerQuery);
        Task<ResponseObject> GetByIdAsync(int id);
        Task<ResponseObject> SaveAsync(BannerModel model);
        Task<ResponseObject> UpdateAsync(int id, BannerModel model);
        Task<ResponseObject> DeleteAsync(int id);
        Task<ResponseObject> DeleteFlagAsync(int id, string deleteBy);
        Task<ResponseObject> GetDeletedListAsync();
        Task<ResponseObject> RestoreAsync(int id);
    }
}
