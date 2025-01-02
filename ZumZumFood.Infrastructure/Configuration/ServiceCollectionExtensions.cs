using ILogger = Serilog.ILogger;
namespace ZumZumFood.Infrastructure.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDerivativeTradeServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
               .AddSerilogConfiguration(configuration)
               .AddSqlServerConfiguration(configuration)
               .AddCorsConfiguration()
               .AddAutoMapperConfiguration()
               .AddSingletonServices()
               .AddEmailConfiguration(configuration)
               .AddJwtConfiguration(configuration)
               .AddCacheConfiguration(configuration)
               .AddOauth2Configuration(configuration)
               .AddTransientServices();
            return services;
        }

        public static void AddTransientServices(this IServiceCollection services)
        {
            services.AddTransient<IPDFService, PDFService>();
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            services.AddTransient<ISQLQueryHandler, SQLQueryHandler>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IRestaurantService, RestaurantService>();
            services.AddTransient<IProductService, ProductService>();
        }

        // Add singleton
        private static IServiceCollection AddSingletonServices(this IServiceCollection services)
        {
            // Đăng ký IHttpContextAccessor
            services.AddHttpContextAccessor();
            return services;
        }

        // Cấu hình dịch vụ email
        public static IServiceCollection AddEmailConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // Bind cấu hình Email từ appsettings.json
            var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailModel>();

            // Đăng ký cấu hình email như một singleton
            services.AddSingleton(emailConfig);
            return services;
        }

        // Đăng ký các dịch vụ scoped
        public static IServiceCollection AddSqlServerConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            return services;
        }

        // Cấu hình AutoMapper
        public static IServiceCollection AddAutoMapperConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutomapConfig));
            return services;
        }

        // Cấu hình CORS
        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin", policy =>
                {
                    policy  // Cho phép nguồn từ localhost:3000
                           .WithOrigins("http://localhost:3000", "http://localhost:4200")
                          .AllowAnyHeader()                         // Cho phép bất kỳ header nào
                          .AllowAnyMethod()                        // Cho phép bất kỳ phương thức HTTP nào
                          .AllowCredentials();                    // Cho phép cookies hoặc thông tin xác thực khác
                });
            });
            return services;
        }

        // Cấu hình logging (Serilog)
        public static IServiceCollection AddSerilogConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory); // Tạo thư mục nếu chưa tồn tại
            }

            // Cấu hình Serilog
            Serilog.Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)  // Đọc cấu hình từ appsettings.json
                .Enrich.FromLogContext()                // Thêm ngữ cảnh log
                .WriteTo.Console()                      // Ghi log ra Console
                .WriteTo.File(
                    Path.Combine(logDirectory, "log-.txt"),  // Đường dẫn tới thư mục LogFileDirectory
                    rollingInterval: RollingInterval.Day,     // Log mỗi ngày vào file mới
                    retainedFileCountLimit: 7                // Giới hạn số file log giữ lại (ví dụ 7 ngày)
                )
                .CreateLogger();
            services.AddSingleton<ILogger>(Serilog.Log.Logger);
            return services;
        }

        public static IServiceCollection AddOauth2Configuration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            })
            .AddGoogle(options =>
            {
                options.ClientId = configuration["Authentication:Google:ClientId"];
                options.ClientSecret = configuration["Authentication:Google:ClientSecret"];
                options.CallbackPath = "/api/v1/auth/google-callback";
                options.SaveTokens = true;
                options.Scope.Add("email");
                options.Scope.Add("profile");
            })
            .AddFacebook(options =>
            {
                options.AppId = configuration["Authentication:Facebook:AppId"];
                options.AppSecret = configuration["Authentication:Facebook:AppSecret"];
                options.Scope.Add("public_profile");
                options.Fields.Add("picture");
                options.Scope.Add("email");
                options.Fields.Add("email");
            });

            return services;
        }

        // Cấu hình bộ nhớ cache (Redis hoặc fallback MemoryCache)
        public static IServiceCollection AddCacheConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // Cấu hình kết nối Redis với fallback sử dụng bộ nhớ nếu Redis không khả dụng
            var redisConnectionString = configuration["CacheConnection:RedisServer"];
            var isRedisConnected = false;
            IConnectionMultiplexer redis = null;

            try
            {
                // Thử kết nối Redis
                redis = ConnectionMultiplexer.Connect(redisConnectionString);
                isRedisConnected = redis.IsConnected;
                Constant.IsRedisConnectedStatic = redis.IsConnected;
            }
            catch (Exception ex)
            {
                // Ghi log nếu không thể kết nối Redis
                Console.WriteLine($"Không thể kết nối Redis: {ex.Message}");
            }
            finally
            {
                redis?.Dispose();
            }

            if (isRedisConnected)
            {
                // Nếu kết nối Redis thành công, sử dụng RedisCache
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = redisConnectionString;
                });

                // Đảm bảo sử dụng RedisCacheService với kết nối Redis
                services.AddScoped<IRedisCacheService, RedisCacheService>(sp =>
                    new RedisCacheService(redisConnectionString, null));
            }
            else
            {
                // Nếu không thể kết nối Redis, sử dụng MemoryCache (fallback)
                Console.WriteLine("Kết nối Redis thất bại, sử dụng MemoryCache.");
                services.AddMemoryCache(); // Thêm MemoryCache nếu không kết nối được Redis
                                           // Đảm bảo fallback sử dụng MemoryCache trong RedisCacheService
                services.AddScoped<IRedisCacheService, RedisCacheService>(sp =>
                    new RedisCacheService(redisConnectionString, sp.GetRequiredService<IMemoryCache>()));
            }
            return services;
        }

        // Cấu hình JWT
        public static IServiceCollection AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
         {
             var key = configuration["Jwt:Key"];
             var signingKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(key));

             services.AddAuthentication(options =>
             {
                 options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                 options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                 options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
             })
             .AddJwtBearer(options =>
             {
                 options.SaveToken = true;
                 options.RequireHttpsMetadata = false;  // Cài đặt này cần bật khi triển khai ứng dụng thực tế
                 options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                 {
                     ValidateIssuer = true,
                     ValidIssuer = configuration["Jwt:Issuer"],  // Lấy thông tin từ appsettings.json

                     ValidateAudience = true,
                     ValidAudience = configuration["Jwt:Audience"],  // Lấy thông tin từ appsettings.json

                     IssuerSigningKey = signingKey,  // Đăng ký khóa ký cho JWT

                     RequireExpirationTime = true,
                     ValidateLifetime = true,  // Kiểm tra thời gian sống của token
                 };

                 options.Events = new JwtBearerEvents
                 {
                     // Xử lý sự kiện khi không có token hoặc token không hợp lệ
                     OnChallenge = context =>
                     {
                         // Bỏ qua phản hồi mặc định của JWT Bearer
                         context.HandleResponse();

                         // Thiết lập trạng thái và kiểu nội dung phản hồi
                         context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                         context.Response.ContentType = "application/json";

                         // Tạo một đối tượng ResponseObject với mã lỗi và thông điệp tùy chỉnh
                         var result = JsonSerializer.Serialize(new ResponseObject(401, "Unauthorized. Token is invalid or missing."));

                         // Trả về phản hồi lỗi tùy chỉnh dưới dạng JSON
                         return context.Response.WriteAsync(result);
                     },

                     // Xử lý sự kiện khi token hợp lệ nhưng người dùng không có quyền truy cập vào tài nguyên
                     OnForbidden = context =>
                     {
                         // Thiết lập trạng thái và kiểu nội dung phản hồi
                         context.Response.StatusCode = StatusCodes.Status403Forbidden;
                         context.Response.ContentType = "application/json";

                         // Tạo một đối tượng ResponseObject với mã lỗi và thông điệp tùy chỉnh
                         var result = JsonSerializer.Serialize(new ResponseObject(403, "Forbidden. You do not have permission to access this resource."));

                         // Trả về phản hồi lỗi tùy chỉnh dưới dạng JSON
                         return context.Response.WriteAsync(result);
                     }
                 };
             });
             return services;
         }
    }
}
