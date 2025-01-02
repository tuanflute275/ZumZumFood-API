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

        [HttpGet("signin-google")]
        public async Task<ActionResult> GoogleLogin()
        {
            
            var redirectUrl = Url.Action("GoogleCallback", "Auth");
            return Challenge(new AuthenticationProperties { RedirectUri = redirectUrl }, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-callback")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ResponseObject> GoogleCallback()
        {
            return await _authService.GoogleCallbackAsync(HttpContext);
        }

        [HttpGet("signin-facebook")]
        public async Task<ActionResult> FacebookLogin()
        {
            var redirectUrl = Url.Action("FacebookCallback", "Auth");
            return Challenge(new AuthenticationProperties { RedirectUri = redirectUrl }, FacebookDefaults.AuthenticationScheme);
        }

        [HttpGet("facebook-callback")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ResponseObject> FacebookCallback()
        {
           return await _authService.FacebookCallbackAsync(HttpContext);
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
