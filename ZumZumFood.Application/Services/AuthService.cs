namespace ZumZumFood.Application.Services
{
    public class AuthService : IAuthService
    {
        IUnitOfWork _unitOfWork;
        private readonly ILogger<AuthService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly IRabbitService _rabbitService;
        public AuthService(
            IUnitOfWork unitOfWork, 
            ILogger<AuthService> logger,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IEmailService emailService,
            IRabbitService rabbitService
            )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _rabbitService = rabbitService;
        }

        public async Task<ResponseObject> LoginAsync(LoginModel model, bool? oauth2 = false)
        {
            try
            {
                // Validate data annotations
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(model, null, null);

                if (!Validator.TryValidateObject(model, validationContext, validationResults, true))
                {
                    var errorMessages = string.Join("; ", validationResults.Select(vr => vr.ErrorMessage));
                    return new ResponseObject(400, "Validation error", errorMessages);
                }
                // end validate

                // kiểm tra tài khoản có tồn tại
                var user = await _unitOfWork.UserRepository.GetByUsernameOrEmailAsync(model.UsernameOrEmail);
                if (user == null)
                {
                    LogHelper.LogWarning(_logger, "Account does not exist.", user.UserName);
                    return new ResponseObject(400, "Account does not exist.");
                }

                // Kiểm tra trạng thái tài khoản
                var lockStatusResponse = CheckAccountLockStatus(user);
                if (lockStatusResponse != null) return lockStatusResponse;

                // Kiểm tra độ dài mật khẩu
                if (model.Password.Length < 6)
                {
                    user.FailedLoginAttempts++;
                    UpdateLockoutStatus(user); // Hàm cập nhật trạng thái khóa

                    await _unitOfWork.UserRepository.SaveOrUpdateAsync(user);
                    LogHelper.LogError(_logger, null, "POST", $"/api/auth/login", "Password must be longer than 6 characters.");
                    return new ResponseObject(400, "Password must be longer than 6 characters.");
                }

                // Kiểm tra mật khẩu có khớp dữ liệu không
                if (!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                {
                    user.FailedLoginAttempts++; // tăng số lần đăng nhập fail
                    UpdateLockoutStatus(user); // Hàm cập nhật trạng thái khóa

                    await _unitOfWork.UserRepository.SaveOrUpdateAsync(user);
                    LogHelper.LogError(_logger, null, "POST", $"/api/auth/login", "Incorrect password.");
                    return new ResponseObject(400, "Incorrect password.");
                }

                // lấy ra quyền của tài khoản
                var roles = await _unitOfWork.UserRoleRepository.GetAllAsync(
                   expression: s => s.UserId == user.UserId,
                   include: query => query.Include(x => x.Role)
                );

                if (roles == null)
                {
                    LogHelper.LogWarning(_logger, "You do not have access.", user.UserName);
                    return new ResponseObject(400, "You do not have access.");
                }

                // generate token and refresh token
                var accessToken = TokenHelper.GenerateJwtToken(user.UserId, user.Email, roles.Select(r => r.Role.RoleName), _configuration);
                var refreshToken = TokenHelper.GenerateRefreshToken();

                //save token vào database
                await ManageTokens(user.UserId, accessToken, refreshToken, (bool)oauth2);

                // Reset trạng thái nếu đã hết thời gian khóa
                ResetLockoutStatus(user);
                await _unitOfWork.UserRepository.SaveOrUpdateAsync(user);
                await _unitOfWork.SaveChangeAsync();

                LogHelper.LogInformation(_logger, "POST", "/api/auth/login", null, "Login successfully");
                return new ResponseObject(200, "Login successfully", new { AccessToken = accessToken, RefreshToken = refreshToken });
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/auth/login");
                return new ResponseObject(500, "Internal server error. Please try again later.", ex.Message);
            }
        }

        // start helper login
        private ResponseObject CheckAccountLockStatus(User user)
        {
            if (user.Active == 2)
            {
                LogHelper.LogWarning(_logger, "Your account has been permanently banned.", user.UserName);
                return new ResponseObject(403, "Your account has been permanently banned.");
            }
            
            if (user.Active == 0)
            {
                if (user.UserUnlockTime.HasValue && DateTime.Now < user.UserUnlockTime.Value)
                {
                    var remainingTime = user.UserUnlockTime.Value - DateTime.Now;
                    int remainingMinutes = (int)remainingTime.TotalMinutes;
                    int remainingSeconds = (int)remainingTime.Seconds;
                    if (remainingMinutes > 0)
                    {
                        LogHelper.LogWarning(_logger, "Account temporarily locked", user.UserName);
                        return new ResponseObject(403, $"Your account is temporarily locked. Try again in {remainingMinutes} minute(s) and {remainingSeconds} second(s).");
                    }
                    else
                    {
                        LogHelper.LogWarning(_logger, "Account temporarily locked", user.UserName);
                        return new ResponseObject(403, $"Your account is temporarily locked. Try again in {remainingSeconds} second(s).");
                    }
                }
            }
            return null; // Trả về null nếu tài khoản không bị khóa
        }

        private void UpdateLockoutStatus(User user)
        {
            double lockDurationInMinutes;
            if (user.FailedLoginAttempts == 3)
            {
                lockDurationInMinutes = 0.5;  // 30 giây
            }
            else if (user.FailedLoginAttempts == 5)
            {
                lockDurationInMinutes = 1;  // 1 phút
            }
            else if (user.FailedLoginAttempts == 6)
            {
                lockDurationInMinutes = 2;  // 2 phút
            }
            else if (user.FailedLoginAttempts == 7)
            {
                lockDurationInMinutes = 3;  // 3 phút
            }
            else if (user.FailedLoginAttempts == 8)
            {
                lockDurationInMinutes = 4;  // 4 phút
            }
            else if (user.FailedLoginAttempts >= 9)
            {
                lockDurationInMinutes = 5;  // 5 phút
            }
            else
            {
                return;
            }

            LockUser(user, lockDurationInMinutes);
        }

        private void LockUser(User user, double minutes)
        {
            user.Active = 0;
            user.UserCurrentTime = DateTime.Now;
            user.UserUnlockTime = DateTime.Now.AddMinutes(minutes);
        }

        private void ResetLockoutStatus(User user)
        {
            user.FailedLoginAttempts = 0;
            user.Active = 1;
            user.UserUnlockTime = null;
            user.UserCurrentTime = null;
        }

        private async Task ManageTokens(int userId, string accessToken, string refreshToken, bool oauth2)
        {
            var userTokens = await _unitOfWork.TokenRepository.GetAllAsync(
                  expression: s => s.UserId == userId
             );

            if (userTokens.Count() >= 3)
            {
                var tokenToDelete = userTokens.OrderBy(t => t.IsMobile).ThenBy(t => t.CreatedAt).First();
                await _unitOfWork.TokenRepository.DeleteAsync(tokenToDelete);
            }

            var request = _httpContextAccessor.HttpContext?.Request;
            string userAgent = request.Headers["User-Agent"].FirstOrDefault();
            bool isMobile = Helpers.IsMobileDevice(userAgent);

            Token token = new Token
            {
                UserId = userId,
                IsMobile = isMobile,
                IsRevoked = false,
                TokenType = oauth2 ? "Oauth2Token" : "AccessToken",
                AccessToken = accessToken,
                ExpirationDate = DateTime.UtcNow.AddHours(1),
                RefreshToken = refreshToken,
                RefreshTokenDate = DateTime.UtcNow.AddDays(15),
                CreatedAt = DateTime.UtcNow
            };
            await _unitOfWork.TokenRepository.SaveOrUpdateAsync(token);
        }
        // end helper login

        public async Task<ResponseObject> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                var storedToken = await _unitOfWork.TokenRepository.GetRefreshTokenAsync(refreshToken);

                if (storedToken == null || storedToken.IsRevoked || storedToken.RefreshTokenDate <= DateTime.UtcNow)
                {
                    LogHelper.LogWarning(_logger, "POST", "/api/auth/refresh-token", null, "Invalid or expired refresh token.");
                    return new ResponseObject(400, "Invalid or expired refresh token.");
                } 

                // Generate new Access Token
                var user = await _unitOfWork.UserRepository.GetByIdAsync(storedToken.UserId);
                var roles = await _unitOfWork.UserRoleRepository.GetAllAsync(
                  expression: s => s.UserId == user.UserId,
                  include: query => query.Include(x => x.Role)
                );

                var newAccessToken = TokenHelper.GenerateJwtToken(
                      user.UserId,
                      user.Email,
                      roles.Select(r => r.Role.RoleName),
                      _configuration
                  );

                // Optionally: Generate a new Refresh Token
                var newRefreshToken = TokenHelper.GenerateRefreshToken();

                // update Data
                storedToken.AccessToken = newAccessToken;
                storedToken.ExpirationDate = DateTime.UtcNow.AddHours(15);
                storedToken.RefreshToken = newRefreshToken;
                storedToken.RefreshTokenDate = DateTime.UtcNow.AddDays(15);
                await _unitOfWork.TokenRepository.SaveOrUpdateAsync(storedToken);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "POST", "/api/auth/refresh-token", null);
                return new ResponseObject(200, "Token refreshed successfully", new
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                });
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", "/api/auth/refresh-token");
                return new ResponseObject(500, "Internal server error. Please try again later.");
            }
        }

        public async Task<ResponseObject> RegisterAsync(RegisterModel model)
        {
            try
            {
                // Validate data annotations
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(model, null, null);

                if (!Validator.TryValidateObject(model, validationContext, validationResults, true))
                {
                    var errorMessages = string.Join("; ", validationResults.Select(vr => vr.ErrorMessage));
                    LogHelper.LogWarning(_logger, "Validation failed for ForgotPassword request", errorMessages);
                    return new ResponseObject(400, "Validation error", errorMessages);
                }
                // end validate

                // Kiểm tra xem username đã tồn tại chưa
                var existingUser = await _unitOfWork.UserRepository.GetByUsernameAsync(model.UserName);
                if (existingUser != null) 
                {
                    LogHelper.LogWarning(_logger, "Username already exists", model.UserName);
                    return new ResponseObject(400, "Username already exists.");
                }

                // Kiểm tra xem email đã tồn tại chưa
                var existingUserEmail = await _unitOfWork.UserRepository.GetByEmailAsync(model.Email);
                if (existingUserEmail != null)
                {
                    LogHelper.LogWarning(_logger, "Email already exists", model.Email);
                    return new ResponseObject(400, "Email already exists.");
                }

                string passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password, 12);
                User user = new User
                {
                    UserName = model.UserName,
                    FullName = model.FullName,
                    Email = model.Email,
                    Password = passwordHash,
                    Avatar = model.Avatar
                };
                await _unitOfWork.UserRepository.SaveOrUpdateAsync(user);
                await _unitOfWork.SaveChangeAsync();
                LogHelper.LogInformation(_logger, "User registered successfully", "/api/register", null, user);

                // Lấy userId vừa tạo
                var userId = user.UserId;
                // Thêm vai trò vào bảng UserRole
                var userRole = new UserRole
                {
                    UserId = userId,
                    RoleId = 5
                };
                await _unitOfWork.UserRoleRepository.SaveOrUpdateAsync(userRole);
                await _unitOfWork.SaveChangeAsync();

                // Gửi email xác nhận đăng ký
                LogHelper.LogInformation(_logger, "GET", "/api/auth/register", null, null);
                await _emailService.SendEmailAsync(model.Email, "Welcome to Our Service", Helpers.BodyRegisterMail(model.FullName));
                return new ResponseObject(200, "Register successfully,please check email!", model);
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/auth/register");
                return new ResponseObject(500, "Internal server error. Please try again later.");
            }
        }
       
        public async Task<ResponseObject> ForgotPasswordAsync(ForgotPasswordModel model)
        {
            try
            {
                // Log the incoming request
                LogHelper.LogInformation(_logger, "POST", "/api/auth/forgot-password", model, null);

                // Validate data annotations
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(model, null, null);

                if (!Validator.TryValidateObject(model, validationContext, validationResults, true))
                {
                    var errorMessages = string.Join("; ", validationResults.Select(vr => vr.ErrorMessage));
                    LogHelper.LogWarning(_logger, "Validation failed for ForgotPassword request", errorMessages);
                    return new ResponseObject(400, "Validation error", errorMessages);
                }

                // Fetch user by email
                var user = await _unitOfWork.UserRepository.GetByEmailAsync(model.Email);
                if (user == null)
                {
                    LogHelper.LogWarning(_logger, "User not found", model.Email);
                    return new ResponseObject(404, "User not found.", null);
                }

                // Generate a random password and hash it
                var pass = Helpers.CreateRandomPassword(8);
                var passHash = BCrypt.Net.BCrypt.HashPassword(pass, 12);

                // Log password generation
                LogHelper.LogInformation(_logger, "Generated new password for user", pass);

                // Update user's password
                user.Password = passHash;
                await _unitOfWork.UserRepository.SaveOrUpdateAsync(user);
                await _unitOfWork.SaveChangeAsync();

                // Log successful password update
                LogHelper.LogInformation(_logger, "Password updated successfully", model.Email);

                // Send email confirmation
                await _emailService.SendEmailAsync(model.Email, "Forgot Password", Helpers.BodyResetPasswordMail(pass));

                // Log email sent
                LogHelper.LogInformation(_logger, "Password reset email sent", model.Email);


                // Send the reset password request to RabbitMQ (Producer)
                var isPublished = await _rabbitService.PublishHNX(model.Email);
                if (!isPublished)
                {
                    LogHelper.LogWarning(_logger, "Failed to publish password reset request to RabbitMQ", model.Email);
                    return new ResponseObject(500, "Failed to process your request, please try again later.");
                }

                // Log the event of sending request to RabbitMQ
                LogHelper.LogInformation(_logger, "Password reset request sent to RabbitMQ", model.Email);

                return new ResponseObject(200, "Forgot password successfully. Please check your email!", model);
            }
            catch (Exception ex)
            {
                // Log the exception details with stack trace and inner exception (if any)
                LogHelper.LogError(_logger, ex, "POST", "/api/auth/forgot-password", new { Email = model.Email });

                return new ResponseObject(500, "Internal server error. Please try again later.");
            }
        }

        public async Task<ResponseObject> UpdateAccountStateAsync(int userId, string action)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
                if (user == null) 
                {
                    LogHelper.LogWarning(_logger, "User not found.", userId.ToString());
                    return new ResponseObject(404, "User not found.");
                }
                if (action.Contains(Constant.ENABLE))
                {
                    user.Active = 1;
                }
                else if (action.Contains(Constant.BLOCK))
                {
                    user.Active = 0;
                }
                else if (action.Contains(Constant.SUSPENDED))
                {
                    user.Active = 2;
                }
                await _unitOfWork.UserRepository.SaveOrUpdateAsync(user);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseObject(200, "Update status active successfully.");
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/auth/register");
                return new ResponseObject(500, "Internal server error. Please try again later.");
            }
        }

        public async Task<ResponseObject> FacebookCallbackAsync(HttpContext httpContext)
        {
            try
            {
                var authenticateResult = await httpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                if (!authenticateResult.Succeeded)
                {
                    _logger.LogError("Facebook authentication failed: {error}", authenticateResult.Failure?.Message);
                    return new ResponseObject(400, "Facebook authentication failed", null);
                }

                var claims = authenticateResult.Principal.Identities.FirstOrDefault()?.Claims;
                var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                var picture = claims?.FirstOrDefault(c => c.Type == "picture")?.Value;

                if (email == null || name == null)
                {
                    return new ResponseObject(404, "Required user information not found", new
                    {
                        Name = name,
                        Email = email
                    });
                }


                // logic login with facebook
                var userCheck = await _unitOfWork.UserRepository.GetByEmailAsync(email);
                // nếu null tức chưa có tài khoản, thì thực hiện tạo tài khoản trước rồi đăng nhập
                if (userCheck == null)
                {
                    var user = new RegisterModel();
                    user.Email = email;
                    user.Avatar = picture;
                    user.UserName = Helpers.GenerateUsernameFromEmail(email);
                    user.FullName = name;
                    user.Password = Constant.DEFAULT_PASSWORD;
                    await RegisterAsync(user);
                }

                // thực hiện login và trả về token
                var login = new LoginModel();
                login.UsernameOrEmail = email;
                login.Password = Constant.DEFAULT_PASSWORD;
                var result = await LoginAsync(login, true);
                return result;
                // end login facebook
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/auth/signin-facebook");
                return new ResponseObject(500, "Internal server error. Please try again later.");
            }
        }

        public async Task<ResponseObject> GoogleCallbackAsync(HttpContext httpContext)
        {
            try
            {
                var authenticateResult = await httpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                if (!authenticateResult.Succeeded)
                {
                    _logger.LogError("Google authentication failed: {error}", authenticateResult.Failure?.Message);
                    return new ResponseObject(400, "Google authentication failed", null);
                }

                var claims = authenticateResult.Principal.Identities.FirstOrDefault()?.Claims;
                var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                var picture = claims?.FirstOrDefault(c => c.Type == "picture")?.Value;

                if (email == null || name == null)
                {
                    return new ResponseObject(404, "Required user information not found", new
                    {
                        Name = name,
                        Email = email
                    });
                }


                // logic login with facebook
                var userCheck = await _unitOfWork.UserRepository.GetByEmailAsync(email);
                // nếu null tức chưa có tài khoản, thì thực hiện tạo tài khoản trước rồi đăng nhập
                if (userCheck == null)
                {
                    var user = new RegisterModel();
                    user.Email = email;
                    user.Avatar = picture;
                    user.UserName = Helpers.GenerateUsernameFromEmail(email);
                    user.FullName = name;
                    user.Password = Constant.DEFAULT_PASSWORD;
                    await RegisterAsync(user);
                }

                // thực hiện login và trả về token
                var login = new LoginModel();
                login.UsernameOrEmail = email;
                login.Password = Constant.DEFAULT_PASSWORD;
                var result = await LoginAsync(login, true);
                return result;
                // end login facebook
            }
            catch (Exception ex)
            {
                LogHelper.LogError(_logger, ex, "POST", $"/api/auth/signin-facebook");
                return new ResponseObject(500, "Internal server error. Please try again later.");
            }
        }
    }
}
