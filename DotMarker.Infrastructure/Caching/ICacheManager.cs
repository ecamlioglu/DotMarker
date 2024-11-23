namespace DotMarker.Infrastructure.Caching;

public interface ICacheManager
{
    T GetOrSet<T>(string key, Func<T> fetch, TimeSpan expiration);
    void Remove(string key);
}