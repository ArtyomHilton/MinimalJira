using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MinimalJira.Domain.Entities;
using Task = MinimalJira.Domain.Entities.Task;

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
    
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Task> Tasks => Set<Task>();
}