using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MinimalJira.Application.DTOs;
using MinimalJira.Persistence.Context;

namespace MinimalJira.Application.UseCases.Project.GetProjects;

public class GetProjectsUseCase(MinimalJiraDbContext dbContext, ILogger<GetProjectsUseCase> logger) : IGetProjectsUseCase
{
    public async Task<ICollection<ProjectDataResponse>> ExecuteAsync(GetProjectsQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Началось получение проектов на страницу {Page} в количестве {PageSize}", query.Page, query.PageSize);
        
        return await dbContext.Projects
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
            .OrderBy(x=> x.CreatedAt)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);
    }
}