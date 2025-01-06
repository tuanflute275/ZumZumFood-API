namespace ZumZumFood.Infrastructure.Services
{
    public class RabbitSetting
    {
        public string Id { get; set; }
        public string QueueName { get; set; }
        public string QueueNameHNX { get; set; }
        public string QueueNameHSX { get; set; }
        public string ExchangeName { get; set; }
        public string RoutingKey { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ExchangeType { get; set; }
        public string Durable { get; set; }
    }
}
