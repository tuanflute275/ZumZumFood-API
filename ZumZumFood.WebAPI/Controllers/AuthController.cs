using Microsoft.AspNetCore.Mvc;
using ZumZumFood.Application.Abstracts;
using ZumZumFood.Application.Models.Request;
using ZumZumFood.Application.Models.Response;

namespace ZumZumFood.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ResponseObject> Login([FromBody] LoginRequestModel model)
        {
            return await _authService.LoginAsync(model);
        }

        [HttpPost("register")]
        public async Task<ResponseObject> Register([FromBody] RegisterRequestModel model)
        {
            return await _authService.RegisterAsync(model);
        }
       
        [HttpPost("refresh-token")]
        public async Task<ResponseObject> refreshToken(string refreshToken)
        {
            return await _authService.RefreshTokenAsync(refreshToken);
        }

        [HttpPost("forgot-password")]
        public async Task<ResponseObject> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            return await _authService.ForgotPasswordAsync(model);
        }

        [HttpPost("user/{userId}/block")]
        public async Task<ResponseObject> BlockUser(int userId)
        {
            return await _authService.UpdateAccountStateAsync(userId, "Block");
        }

        [HttpPost("user/{userId}/enable")]
        public async Task<ResponseObject> EnableUser(int userId)
        {
            return await _authService.UpdateAccountStateAsync(userId, "Enable");
        }

        [HttpPost("user/{userId}/suspended")]
        public async Task<ResponseObject> SuspendedUser(int userId)
        {
            return await _authService.UpdateAccountStateAsync(userId, "Suspended");
        }
    }
}
