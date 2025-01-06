namespace ZumZumFood.Application.Abstracts
{
    public interface ICouponConditionService
    {
        Task<ResponseObject> GetByIdAsync(int id);
        Task<ResponseObject> SaveAsync(CouponConditionModel model);
        Task<ResponseObject> UpdateAsync(int id, CouponConditionModel model);
        Task<ResponseObject> DeleteAsync(int id);
    }
}
