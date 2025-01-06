namespace ZumZumFood.Application.Services.RabbitMQ
{
    public class RabbitMqConsumer
    {
        private readonly RabbitSetting _rabbitMqSettings;
        private readonly ILogger<RabbitMqConsumer> _logger;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMqConsumer(RabbitSetting rabbitMqSettings, ILogger<RabbitMqConsumer> logger)
        {
            _rabbitMqSettings = rabbitMqSettings;
            _logger = logger;
        }


        public void StartConsuming(Action<string> onMessageReceived)
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _rabbitMqSettings.HostName,
                    UserName = _rabbitMqSettings.UserName,
                    Password = _rabbitMqSettings.Password,
                    Port = _rabbitMqSettings.Port
                };

                // Tạo kết nối và channel
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                // Đảm bảo rằng Queue đã được khai báo
                _channel.QueueDeclare(queue: _rabbitMqSettings.QueueNameHNX,
                                      durable: false,
                                      exclusive: false,
                                      autoDelete: false,
                                      arguments: null);

                // Tạo consumer
                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    onMessageReceived?.Invoke(message);
                };

                // Bắt đầu nhận thông điệp
                _channel.BasicConsume(queue: _rabbitMqSettings.QueueNameHNX,
                                      autoAck: true,
                                      consumer: consumer);

                _logger.LogInformation("Started consuming from RabbitMQ queue: {QueueName}", _rabbitMqSettings.QueueNameHNX);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while starting consumer: {Error}", ex.Message);
            }
        }

        public void StopConsuming()
        {
            // Đóng kết nối và channel khi ngừng tiêu thụ
            _channel?.Close();
            _connection?.Close();

            _logger.LogInformation("Stopped consuming from RabbitMQ queue: {QueueName}", _rabbitMqSettings.QueueNameHNX);
        }
    }
}
