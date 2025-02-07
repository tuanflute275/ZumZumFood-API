namespace ZumZumFood.Application.Abstracts
{
    public interface ICouponService
    {
        Task<ResponseObject> GetAllPaginationAsync(CouponQuery couponQuery);
        Task<ResponseObject> GetByIdAsync(int id);
        Task<ResponseObject> SaveAsync(CouponModel model);
        Task<ResponseObject> UpdateAsync(int id, CouponModel model);
        Task<ResponseObject> DeleteAsync(int id);
        Task<ResponseObject> CalculateCouponValueAsync(string couponCode, double? totalAmount,
            string? currentCategory, int? currentQuantity, string? currentUserType,
            string? currentPaymentMethod, string? currentBrand, int? currentOrderCount);
    }
}
