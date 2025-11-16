using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MinimalJira.Application.DTOs;
using MinimalJira.Application.Interfaces;
using MinimalJira.Persistence.Context;

namespace MinimalJira.Application.UseCases.Project.GetProjects;

public class GetProjectsUseCase(
    MinimalJiraDbContext dbContext,
    ICacheService cacheService,
    ILogger<GetProjectsUseCase> logger) : IGetProjectsUseCase
{
    public async Task<ICollection<ProjectDataResponse>> ExecuteAsync(GetProjectsQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Началось получение проектов на страницу {Page} в количестве {PageSize}", query.Page,
            query.PageSize);

        var key = $"projects_page_{query.Page}_pageSize_{query.PageSize}";

        var cacheProjects = await cacheService.GetAsync<ICollection<ProjectDataResponse>>(key, cancellationToken);

        if (cacheProjects is not null)
        {
            return cacheProjects;
        }

        var projects = await dbContext.Projects
            .Include(p => p.Tasks)
            .Select(p => new ProjectDataResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                CreatedAt = p.CreatedAt,
                Tasks = p.Tasks.Select(t => new TaskDataResponse
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    CreatedAt = t.CreatedAt,
                    IsComplete = t.IsComplete
                }).ToList()
            })
            .OrderBy(x => x.CreatedAt)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        await cacheService.SetAsync(key, projects, 5, cancellationToken);

        return projects;
    }
}