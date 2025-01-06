namespace ZumZumFood.Application.Services.RabbitMQ
{
    public class RabbitService : IRabbitService, IDisposable
    {
        private readonly IModel _modelHNX;
        private readonly IModel _modelFixReceive;
        private readonly ILogger<RabbitService> _logger;
        private readonly RabbitSetting _rabbitHNXSetting;
        private readonly RabbitSetting _rabbitSettingFixReceive;

        public RabbitService(
           ConnectionFactory factoryHNX,
           ConnectionFactory factoryFixReceive,
           ILogger<RabbitService> logger,
           RabbitSetting rabbitHNXSetting,
           RabbitSetting rabbitSettingFixReceive)
        {
            _modelHNX = factoryHNX.CreateConnection().CreateModel();
            _modelFixReceive = factoryFixReceive.CreateConnection().CreateModel();
            _logger = logger;
            _rabbitHNXSetting = rabbitHNXSetting;
            _rabbitSettingFixReceive = rabbitSettingFixReceive;

            // Khai báo Queue và Exchange cho HNX
            DeclareQueueAndExchange(_modelHNX, rabbitHNXSetting);

            // Khai báo Queue và Exchange cho FixReceive
            //DeclareQueueAndExchange(_modelFixReceive, rabbitSettingFixReceive);
        }

        private void DeclareQueueAndExchange(IModel model, RabbitSetting rabbitSetting)
        {
            // Declare Exchange (Direct Exchange trong trường hợp này)
            model.ExchangeDeclare(rabbitSetting.ExchangeName, "direct", durable: true, autoDelete: false);

            // Declare Queue
            model.QueueDeclare(rabbitSetting.QueueNameHNX, durable: true, exclusive: false, autoDelete: false);

            // Bind Queue to Exchange
            model.QueueBind(rabbitSetting.QueueNameHNX, rabbitSetting.ExchangeName, rabbitSetting.RoutingKey);
        }

        public async Task<bool> PublishHNX(string data)
        => await PublishRabbitHNX(data, _rabbitHNXSetting.QueueNameHNX);

        private async Task<bool> PublishRabbitHNX(string data, string queueName)
        {
            try
            {
                if (data is null) return false;
                var message = Encoding.Default.GetBytes(data);
                _modelHNX.BasicPublish("", queueName, null, message);
                return true;
            }
            catch (Exception ex)
            {
                var errorMessage = $"ex={ex};\n\r data={data};";
                _logger.LogError(errorMessage);
                return false;
            }
        }
        public async Task<bool> PublishFixReceive(string data)
        {
            try
            {
                if (data is null) return false;
                var message = Encoding.Default.GetBytes(data);
                _modelFixReceive.BasicPublish("", _rabbitSettingFixReceive.QueueName, null, message);
                return true;
            }
            catch (Exception ex)
            {
                var errorMessage = $"ex={ex};\n\r data={data};";
                _logger.LogError(errorMessage);
                return false;
            }
        }

        public void Dispose()
        {
            _modelHNX?.Dispose();
            _modelFixReceive?.Dispose();
        }
    }
}
