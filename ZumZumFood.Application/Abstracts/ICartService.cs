namespace ZumZumFood.Application.Abstracts
{
    public interface ICartService
    {
        Task<ResponseObject> GetByIdAsync(int id);
        Task<ResponseObject> SaveAsync(CartModel model);
        Task<ResponseObject> UpdateAsync(int id, string type);
        Task<ResponseObject> DeleteAsync(int id);
    }
}
