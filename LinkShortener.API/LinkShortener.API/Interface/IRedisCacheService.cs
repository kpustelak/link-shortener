using Microsoft.Extensions.Caching.Distributed;

namespace LinkShortener.API.Interface;

public interface IRedisCacheService
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
    Task RemoveAsync(string key);
}