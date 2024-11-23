using Microsoft.Extensions.Caching.Memory;

namespace DotMarker.Infrastructure.Caching;

public class CacheManager: ICacheManager
{
    private readonly IMemoryCache _cache;

    public CacheManager(IMemoryCache cache)
    {
        _cache = cache;
    }

    public T GetOrSet<T>(string key, Func<T> fetch, TimeSpan expiration)
    {
        if (!_cache.TryGetValue(key, out T value))
        {
            value = fetch();
            _cache.Set(key, value, expiration);
        }
        return value;
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
    }
}