namespace ZumZumFood.Application.Services
{
    public class RabbitMqConsumer
    {
        private readonly RabbitSetting _rabbitMqSettings;

        public RabbitMqConsumer(RabbitSetting rabbitMqSettings)
        {
            _rabbitMqSettings = rabbitMqSettings;
        }

        public void StartConsuming(Action<string> onMessageReceived)
         {
             var factory = new ConnectionFactory()
             {
                 HostName = _rabbitMqSettings.HostName,
                 UserName = _rabbitMqSettings.UserName,
                 Password = _rabbitMqSettings.Password
             };

             var connection = factory.CreateConnection();
             var channel = connection.CreateModel();

             channel.QueueDeclare(queue: _rabbitMqSettings.QueueName,
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

             var consumer = new EventingBasicConsumer(channel);
             consumer.Received += (model, ea) =>
             {
                 var body = ea.Body.ToArray();
                 var message = Encoding.UTF8.GetString(body);
                 onMessageReceived?.Invoke(message);
             };

             channel.BasicConsume(queue: _rabbitMqSettings.QueueName,
                                  autoAck: true,
                                  consumer: consumer);
         }
    }
}
