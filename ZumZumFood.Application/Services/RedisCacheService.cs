using ZumZumFood.Application.Utils.Common;

namespace ZumZumFood.Application.Services
{
    public class RedisCacheService : IDisposable, IRedisCacheService
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;
        private IMemoryCache _memoryCache;

        public RedisCacheService(string connectionString, IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            if (Constant.IsRedisConnectedStatic) // Kiểm tra trạng thái kết nối Redis
            {
                _redis = ConnectToRedis(connectionString);
                _database = _redis?.GetDatabase();
            }
            else
            {
                Console.WriteLine("Redis không khả dụng. Sử dụng MemoryCache.");
            }
        }

        private ConnectionMultiplexer ConnectToRedis(string connectionString)
        {
            try
            {
                var modifiedConnectionString = $"{connectionString},abortConnect=false";
                var connection = ConnectionMultiplexer.Connect(modifiedConnectionString);
                Console.WriteLine("Kết nối Redis thành công.");
                return connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Không thể kết nối Redis: {ex.Message}");
                return RetryConnect(connectionString);
            }
        }

        private ConnectionMultiplexer RetryConnect(string connectionString)
        {
            int maxRetries = 3;
            for (int attempt = 0; attempt < maxRetries; attempt++)
            {
                try
                {
                    return ConnectionMultiplexer.Connect(connectionString);
                }
                catch (Exception)
                {
                    if (attempt == maxRetries - 1) throw;
                    Thread.Sleep(1000); // Đợi 1 giây trước khi thử lại
                }
            }
            throw new Exception("Không thể kết nối Redis sau khi thử nhiều lần.");
        }

        private bool IsRedisConnected()
        {
            return _redis?.IsConnected ?? false;
        }

        // Lưu trữ vào Redis hoặc MemoryCache
        public async Task SetCacheAsync(string key, string value, TimeSpan? expiration = null)
        {
            if (IsRedisConnected()) // Sử dụng Redis nếu có kết nối
            {
                await _database.StringSetAsync(key, value, expiration);
            }
            else // Nếu không có kết nối Redis, sử dụng MemoryCache
            {
                _memoryCache.Set(key, value, expiration ?? TimeSpan.FromMinutes(30));
            }
        }

        // Xóa dữ liệu khỏi Redis hoặc MemoryCache
        public async Task RemoveCacheAsync(string key)
        {
            if (IsRedisConnected())
            {
                await _database.KeyDeleteAsync(key);
            }
            else
            {
                _memoryCache.Remove(key);
            }
        }

        public async Task ClearCacheAsync()
        {
            if (IsRedisConnected())
            {
                // Xóa tất cả dữ liệu trong Redis
                await _database.ExecuteAsync("FLUSHALL");
            }
            else
            {
                // Xóa tất cả dữ liệu trong MemoryCache bằng cách tạo lại instance của MemoryCache
                _memoryCache = new MemoryCache(new MemoryCacheOptions());
            }
        }

        // Kiểm tra sự tồn tại của cache trong Redis hoặc MemoryCache
        public async Task<bool> ExistsCacheAsync(string key)
        {
            if (IsRedisConnected())
            {
                return await _database.KeyExistsAsync(key);
            }

            else
            {
                return _memoryCache.TryGetValue(key, out _);
            }
        }

        // Lấy dữ liệu từ Redis hoặc MemoryCache
        public async Task<string> GetCacheAsync(string key)
        {
            if (IsRedisConnected()) // Sử dụng Redis nếu có kết nối
            {
                return await _database.StringGetAsync(key);
            }
            else // Nếu không có kết nối Redis, lấy từ MemoryCache
            {
                _memoryCache.TryGetValue(key, out string value);
                return value;
            }
        }

        // Lấy dữ liệu từ Redis hoặc MemoryCache với Locking cơ chế
        public async Task<string> GetCacheWithLockAsync(string key)
        {
            if (IsRedisConnected())
            {
                // Kiểm tra dữ liệu trong Redis
                var cachedValue = await _database.StringGetAsync(key);
                if (!string.IsNullOrEmpty(cachedValue))
                {
                    return cachedValue;
                }

                // Kiểm tra khóa lock trong Redis
                var lockKey = $"{key}:lock";
                var lockValue = Guid.NewGuid().ToString(); // Đảm bảo khóa là duy nhất
                var lockExpiry = TimeSpan.FromSeconds(30); // Thời gian sống của lock
                var isLockAcquired = await _database.StringSetAsync(lockKey, lockValue, lockExpiry, When.NotExists);
                if (isLockAcquired)
                {
                    try
                    {
                        // Sau khi đặt lock, kiểm tra lại dữ liệu trong Redis
                        cachedValue = await _database.StringGetAsync(key);
                        if (!string.IsNullOrEmpty(cachedValue))
                        {
                            return cachedValue;
                        }
                        return null;
                    }
                    finally
                    {
                        // Giải phóng khóa
                        var currentLockValue = await _database.StringGetAsync(lockKey);
                        if (currentLockValue == lockValue)
                        {
                            await _database.KeyDeleteAsync(lockKey);
                        }
                    }
                }
                else
                {
                    // Nếu không đặt được khóa, đợi ngắn rồi thử lại
                    await Task.Delay(1000); // Giảm thời gian chờ để cải thiện hiệu suất
                    return await GetCacheWithLockAsync(key);
                }

            }

            // Nếu Redis không kết nối, sử dụng MemoryCache
            _memoryCache.TryGetValue(key, out string value);
            return value;
        }

        public void Dispose()
        {
            _redis?.Dispose();
        }
    }
}
