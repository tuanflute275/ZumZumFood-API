namespace ZumZumFood.Infrastructure.Services
{
    public class RabbitService : IRabbitService
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
        }

        public void PublishHNX(string data)
        => PublishRabbitHNX(data, _rabbitHNXSetting.QueueNameHNX);

        private void PublishRabbitHNX(string data, string queueName)
        {
            try
            {
                if (data is null) return;
                var message = Encoding.Default.GetBytes(data);
                _modelHNX.BasicPublish("", queueName, null, message);
            }
            catch (Exception ex)
            {
                var errorMessage = $"ex={ex};\n\r data={data};";
                _logger.LogError(errorMessage);
            }
        }
        public void PublishFixReceive(string data)
        {
            try
            {
                if (data is null) return;
                   var message = Encoding.Default.GetBytes(data);
                  _modelFixReceive.BasicPublish("", _rabbitSettingFixReceive.QueueName, null, message);
            }
            catch (Exception ex)
            {
                var errorMessage = $"ex={ex};\n\r data={data};";
                _logger.LogError(errorMessage);
            }
        }
    }
}
