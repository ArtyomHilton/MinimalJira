using Microsoft.EntityFrameworkCore;
using MinimalJira.Application.DTOs;
using MinimalJira.Domain.Exceptions;
using MinimalJira.Persistence.Context;

namespace MinimalJira.Application.UseCases.Project.GetProjectById;

public class GetProjectByIdUseCase(MinimalJiraDbContext dbContext) : IGetProjectByIdUseCase
{
    public async Task<ProjectDataResponse> ExecuteAsync(GetProjectByIdQuery query, CancellationToken cancellationToken)
    {
        var project = await dbContext.Projects
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
            }).FirstOrDefaultAsync(p => p.Id == query.Id, cancellationToken);
        if (project is null)
        {
            throw new NotFoundException($"Проект c Id: {query.Id} не найден!");
        }

        return project;
    }
}