using Microsoft.Extensions.Hosting;
namespace ZumZumFood.Application.Services.RabbitMQ
{
    public class RabbitMqBackgroundService : BackgroundService
    {
        private readonly RabbitMqConsumer _rabbitMqConsumer;
        private readonly ILogger<RabbitMqBackgroundService> _logger;

        public RabbitMqBackgroundService(RabbitMqConsumer rabbitMqConsumer, ILogger<RabbitMqBackgroundService> logger)
        {
            _rabbitMqConsumer = rabbitMqConsumer;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Sử dụng background task để bắt đầu tiêu thụ tin nhắn từ RabbitMQ
            _logger.LogInformation("Starting RabbitMQ consumer...");
            _rabbitMqConsumer.StartConsuming(message =>
            {
                // Xử lý khi có tin nhắn nhận được
                _logger.LogInformation($"Message received: {message}");

                // Bạn có thể xử lý thêm logic khi nhận được tin nhắn ở đây.
            });

            return Task.CompletedTask; // Kết thúc task
        }
    }
}
