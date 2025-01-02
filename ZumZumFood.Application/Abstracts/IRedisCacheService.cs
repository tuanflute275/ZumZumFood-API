namespace ZumZumFood.Application.Abstracts
{
    public interface IRedisCacheService
    {
        Task ClearCacheAsync();
        void Dispose();
        Task<bool> ExistsCacheAsync(string key);
        Task<string> GetCacheAsync(string key);
        Task<string> GetCacheWithLockAsync(string key);
        Task RemoveCacheAsync(string key);
        Task SetCacheAsync(string key, string value, TimeSpan? expiration = null);
    }
}