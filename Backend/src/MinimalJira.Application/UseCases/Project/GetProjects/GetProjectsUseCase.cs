using Microsoft.EntityFrameworkCore;
using MinimalJira.Application.DTOs;
using MinimalJira.Persistence.Context;

namespace MinimalJira.Application.UseCases.Project.GetProjects;

public class GetProjectsUseCase(MinimalJiraDbContext dbContext) : IGetProjectsUseCase
{
    public async Task<ICollection<ProjectDataResponse>> ExecuteAsync(GetProjectsQuery query,
        CancellationToken cancellationToken) => await dbContext.Projects
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
        }).Skip((query.Page - 1) * query.PageSize)
        .Take(query.PageSize)
        .ToListAsync(cancellationToken);
}