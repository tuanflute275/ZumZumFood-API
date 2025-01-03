namespace ZumZumFood.Application.Abstracts
{
    public interface IProductCommentService
    {
        Task<ResponseObject> GetByIdAsync(int id);
        Task<ResponseObject> SaveAsync(ProductCommentRequestModel model);
        Task<ResponseObject> UpdateAsync(int id, ProductCommentUpdateRequestModel model);
        Task<ResponseObject> DeleteAsync(int id);
    }
}
