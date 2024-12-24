using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Todo.Core.Interfaces;
using Todo.Infrastructure.Configurations;

namespace Todo.Infrastructure.Services;

public class RedisCachingService(IDistributedCache cache) : IRedisCacheService
{
    public async Task<T?> GetData<T>(string key)
    {
        var data = await cache.GetStringAsync(key);
        return string.IsNullOrEmpty(data) ? default : JsonSerializer.Deserialize<T>(data);
    }

    public async Task SetData<T>(string key, T data)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(Constants.TimeSpanByMinutesForCaching),
            SlidingExpiration = TimeSpan.FromMinutes(Constants.TimeSpanByMinutesForCaching)
        };
        await cache.SetStringAsync(key, JsonSerializer.Serialize(data), options);
    }

    public async Task<T> UpdateData<T>(string key, T data)
    {
        await cache.RemoveAsync(key);
        await SetData(key, data);
        return data;
    }

    public async Task RemoveData(string key) => await cache.RemoveAsync(key);
}