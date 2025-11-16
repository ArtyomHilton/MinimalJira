using Microsoft.EntityFrameworkCore;
using MinimalJira.Domain.Entities;
using MinimalJira.Persistence.Context;
using Task = System.Threading.Tasks.Task;

namespace MinimalJira.Persistence.Seeding;

/// <summary>
/// Сидер данных в БД.
/// </summary>
public static class SeedingProjectsWithTasks
{
    public static void SeedingConfigure(DbContextOptionsBuilder builder)
    {
        builder.UseAsyncSeeding(async (context, shouldSeed, cancellationToken) =>
        {
            var dbContext = (MinimalJiraDbContext)context;
            await SeedProjectAndTasks(dbContext, cancellationToken);
        });
    }

    private static async Task SeedProjectAndTasks(MinimalJiraDbContext context, CancellationToken cancellationToken)
    {
        if (await context.Projects.AnyAsync(cancellationToken))
        {
            return;
        }

        var projects = new List<Project>();

        for (int i = 1; i <= 10; i++)
        {
            projects.Add(CreateProjectWithTask(i));
        }

        await context.Projects.AddRangeAsync(projects, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    private static Project CreateProjectWithTask(int projectNumber)
    {
        var project = new Project()
        {
            Id = Guid.NewGuid(),
            Name = $"Проект_{projectNumber}",
            Description = $"Описание_{projectNumber}",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        for (var i = 1; i <= projectNumber * 2; i++)
        {
            project.Tasks.Add(new Domain.Entities.Task()
            {
                Id = Guid.NewGuid(),
                Title = $"Задача_{i}_проект_{projectNumber}",
                Description = $"Задача_{i}_описание",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsComplete = i % 2 == 0,
                ProjectId = project.Id
            });
        }

        return project;
    }
}