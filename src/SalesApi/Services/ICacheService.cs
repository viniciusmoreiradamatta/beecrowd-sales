namespace SalesApi.Services;

public interface ICacheService
{
    Task<T> GetAsync<T>(string cacheKey, CancellationToken cancellationToken);

    Task SetAsync<T>(string cacheKey, T value, CancellationToken cancellationToken);
}
