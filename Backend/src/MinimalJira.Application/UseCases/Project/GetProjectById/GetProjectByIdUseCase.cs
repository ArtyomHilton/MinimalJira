using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MinimalJira.Application.DTOs;
using MinimalJira.Domain.Exceptions;
using MinimalJira.Persistence.Context;

namespace MinimalJira.Application.UseCases.Project.GetProjectById;

public class GetProjectByIdUseCase(MinimalJiraDbContext dbContext, ILogger<GetProjectByIdUseCase> logger)
    : IGetProjectByIdUseCase
{
    public async Task<ProjectDataResponse> ExecuteAsync(GetProjectByIdQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Началось получение проекта с Id: {Id}", query.Id);

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
            }).AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == query.Id, cancellationToken);
        
        if (project is null)
        {
            logger.LogInformation("Не удалось найти проект с Id: {Id}", query.Id);
            throw new NotFoundException($"Проект c Id: {query.Id} не найден!");
        }

        logger.LogInformation("Проект с Id: {Id} успешно найден", query.Id);

        return project;
    }
}