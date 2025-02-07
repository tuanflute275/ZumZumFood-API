namespace ZumZumFood.Application.Abstracts
{
    public interface IOrderService
    {
        Task<ResponseObject> GetAllPaginationAsync(OrderQuery orderQuery);
        Task<ResponseObject> GetByIdAsync(int id);
        Task<ResponseObject> SaveAsync(OrderModel model);
        Task<ResponseObject> UpdateAsync(int id, OrderUpdateModel model);
        Task<ResponseObject> DeleteAsync(int id);
    }
}
