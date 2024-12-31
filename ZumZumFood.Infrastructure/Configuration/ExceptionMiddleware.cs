using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using ZumZumFood.Application.Utils;

namespace ZumZumFood.Infrastructure.Configuration
{
    // Middleware này sẽ bắt tất cả các ngoại lệ không xử lý trong ứng dụng và trả về phản hồi phù hợp.
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                // Log the exception details
                LogHelper.LogError(_logger, ex, "ExceptionMiddleware", httpContext.Request.Path, ex.Message);
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = new object();

            // Kiểm tra lỗi UnauthorizedAccessException (Lỗi 401)
            if (exception is UnauthorizedAccessException || context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                response = new
                {
                    status = StatusCodes.Status401Unauthorized,
                    message = "Unauthorized Access"
                };
            }
            // Kiểm tra lỗi Forbidden (Lỗi 403)
            else if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                response = new
                {
                    status = StatusCodes.Status403Forbidden,
                    message = "Forbidden: You do not have permission to access this resource."
                };
            }
            // Các lỗi còn lại sẽ trả về lỗi server nội bộ (500)
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                response = new
                {
                    status = StatusCodes.Status500InternalServerError,
                    message = "Internal Server Error",
                    detailed = _env.IsDevelopment() ? exception.Message : null // Hiển thị chi tiết lỗi khi đang phát triển
                };
            }

            context.Response.ContentType = "application/json";
            var jsonResponse = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }
    }
}
