using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace DotMarker.Infrastructure.Caching;

public class CacheManager: ICacheManager
{
    private readonly IDistributedCache _cache;

    public CacheManager(IDistributedCache cache)
    {
        _cache = cache;
    }

    public T GetOrSet<T>(string key, Func<T> fetch, TimeSpan expiration)
    {
        var cachedData = _cache.GetString(key);
        if (cachedData != null)
        {
            return JsonSerializer.Deserialize<T>(cachedData);
        }

        var value = fetch();
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        };
        _cache.SetString(key, JsonSerializer.Serialize(value), options);
        return value;
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
    }
}