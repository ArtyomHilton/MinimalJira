using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinimalJira.Application.Interfaces;
using MinimalJira.Infrastructure.Services;

namespace MinimalJira.Infrastructure.Extensions;

/// <summary>
/// Внедрение зависимостей.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Добавляет и конфигурирует кэширование.
    /// </summary>
    /// <param name="serviceCollection"><see cref="IServiceCollection"/></param>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <returns>Модифицированный <paramref name="serviceCollection"/></returns>
    public static IServiceCollection AddDistributedCache(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
            options.InstanceName = "MinimalJira_";
        });

        serviceCollection.AddSingleton<ICacheService, CacheService>();

        return serviceCollection;
    }
}