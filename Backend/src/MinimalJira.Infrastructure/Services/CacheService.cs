using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using MinimalJira.Application.Interfaces;

namespace MinimalJira.Infrastructure.Services;

/// <summary>
///  <inheritdoc cref="ICacheService"/>
/// </summary>
public class CacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;

    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public CacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = false,
            PropertyNameCaseInsensitive = false
        };
    }
    
    /// <inheritdoc />
    public async Task SetAsync<T>(string key, T value, int expiresByMinutes, CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(value, _jsonSerializerOptions);

        await _distributedCache.SetStringAsync(key, json, new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expiresByMinutes)
        }, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken)
    {
        var json = await _distributedCache.GetStringAsync(key, cancellationToken);

        return json is null 
            ? default(T)
            : JsonSerializer.Deserialize<T>(json, _jsonSerializerOptions);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(string key, CancellationToken cancellationToken)
    {
        await _distributedCache.RemoveAsync(key, cancellationToken);
    }
}