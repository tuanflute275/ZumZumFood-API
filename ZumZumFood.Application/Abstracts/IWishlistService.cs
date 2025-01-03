namespace ZumZumFood.Application.Abstracts
{
    public interface IWishlistService
    {
        Task<ResponseObject> GetByIdAsync(int id);
        Task<ResponseObject> SaveAsync(WishlistModel model);
        Task<ResponseObject> UpdateAsync(int id, WishlistModel model);
        Task<ResponseObject> DeleteAsync(int id);
    }
}
