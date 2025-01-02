namespace ZumZumFood.Application.Abstracts
{
    public interface IAuthService
    {
        Task<ResponseObject> LoginAsync(LoginRequestModel model, bool? oauth2 = false);
        Task<ResponseObject> RegisterAsync(RegisterRequestModel model);
        Task<ResponseObject> RefreshTokenAsync(string refreshToken);
        Task<ResponseObject> ForgotPasswordAsync(ForgotPasswordModel model);
        Task<ResponseObject> UpdateAccountStateAsync(int userId, string action);
        Task<ResponseObject> FacebookCallbackAsync(HttpContext httpContext);
        Task<ResponseObject> GoogleCallbackAsync(HttpContext httpContext);
    }
}
