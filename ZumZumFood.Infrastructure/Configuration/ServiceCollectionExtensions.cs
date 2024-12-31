using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text.Json;
using ZumZumFood.Application.Abstracts;
using ZumZumFood.Application.Configuration;
using ZumZumFood.Application.Models.Request;
using ZumZumFood.Application.Models.Response;
using ZumZumFood.Application.Services;
using ZumZumFood.Domain.Abstracts;
using ZumZumFood.Infrastructure.Abstracts;
using ZumZumFood.Infrastructure.Services;
using ZumZumFood.Persistence.Data;
using ZumZumFood.Persistence.Repositories;


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
               .AddTransientServices();
            return services;
        }

        public static void AddTransientServices(this IServiceCollection services)
        {
            services.AddTransient<IPDFService, PDFService>();
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IUserService, UserService>();
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
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)  // Đọc cấu hình từ appsettings.json
                .Enrich.FromLogContext()                // Thêm ngữ cảnh log
                .WriteTo.Console()                      // Ghi log ra Console
                .WriteTo.File(
                    Path.Combine(logDirectory, "log-.txt"),  // Đường dẫn tới thư mục LogFileDirectory
                    rollingInterval: RollingInterval.Day,     // Log mỗi ngày vào file mới
                    retainedFileCountLimit: 7                // Giới hạn số file log giữ lại (ví dụ 7 ngày)
                )
                .CreateLogger();
            services.AddSingleton<ILogger>(Log.Logger);
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
