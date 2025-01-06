namespace ZumZumFood.Infrastructure.Abstracts
{
    public interface IRabbitService
    {
        Task<bool> PublishHNX(string data);
        Task<bool> PublishFixReceive(string data);
    }
}
