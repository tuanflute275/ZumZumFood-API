namespace ZumZumFood.Infrastructure.Abstracts
{
    public interface IRabbitService
    {
        void PublishHNX(string data);
        void PublishFixReceive(string data);
    }
}
