using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace MinimalJira.Persistence.Context;

/// <summary>
/// Контекст базы данных.
/// </summary>
public class MinimalJiraDbContext : DbContext
{
    public MinimalJiraDbContext(DbContextOptions<MinimalJiraDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}