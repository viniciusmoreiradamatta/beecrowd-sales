using Microsoft.Extensions.Caching.Distributed;
using SalesDomain.Extensions;

namespace SalesApi.Services;

public class CacheService(IDistributedCache cache) : ICacheService
{
    public async Task<T> GetAsync<T>(string cacheKey, CancellationToken cancellationToken)
    {
        var cachedResponse = await cache.GetStringAsync(cacheKey, cancellationToken);

        return string.IsNullOrEmpty(cachedResponse) ? default : cachedResponse.Deserialize<T>();
    }

    public async Task SetAsync<T>(string cacheKey, T value, CancellationToken cancellationToken)
    {
        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
            SlidingExpiration = TimeSpan.FromMinutes(2)
        };

        await cache.SetStringAsync(cacheKey, value.Serialize(), cacheOptions, cancellationToken);
    }
}