using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinimalJira.Persistence.Context;
using MinimalJira.Persistence.Seeding;

namespace MinimalJira.Persistence.Extensions;

/// <summary>
/// Внедрение зависимостей.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Регистрирует и конфигурирует контекст базы данных.
    /// </summary>
    /// <param name="serviceCollection"><see cref="IServiceCollection"/></param>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <returns>Модифицированный <paramref name="serviceCollection"/></returns>
    public static IServiceCollection AddDbContext(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddDbContext<MinimalJiraDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("PostgreSQL"),
                builder =>
                {
                    builder.EnableRetryOnFailure(5);
                });
            SeedingProjectsWithTasks.SeedingConfigure(options);
        });

        return serviceCollection;
    }
}