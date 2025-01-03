namespace ZumZumFood.Application.Abstracts
{
    public interface IAuthService
    {
        Task<ResponseObject> LoginAsync(LoginModel model, bool? oauth2 = false);
        Task<ResponseObject> RegisterAsync(RegisterModel model);
        Task<ResponseObject> RefreshTokenAsync(string refreshToken);
        Task<ResponseObject> ForgotPasswordAsync(ForgotPasswordModel model);
        Task<ResponseObject> UpdateAccountStateAsync(int userId, string action);
        Task<ResponseObject> FacebookCallbackAsync(HttpContext httpContext);
        Task<ResponseObject> GoogleCallbackAsync(HttpContext httpContext);
    }
}
