using Microsoft.EntityFrameworkCore;
using MinimalJira.Persistence.Context;

namespace MinimalJira.Host.Extensions;

public static class WebApplicationExtensions
{
    public static void ApplyMigrations(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<MinimalJiraDbContext>();
        service.Database.Migrate();
    }
}