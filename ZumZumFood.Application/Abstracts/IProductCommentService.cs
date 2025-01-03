namespace ZumZumFood.Application.Abstracts
{
    public interface IProductCommentService
    {
        Task<ResponseObject> GetByIdAsync(int id);
        Task<ResponseObject> SaveAsync(ProductCommentModel model);
        Task<ResponseObject> UpdateAsync(int id, ProductCommentUpdateModel model);
        Task<ResponseObject> DeleteAsync(int id);
    }
}
