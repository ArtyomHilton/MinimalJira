using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MinimalJira.Domain.Exceptions;
using MinimalJira.Persistence.Context;

namespace MinimalJira.Application.UseCases.Task.UpdateTask;

public class UpdateTaskUseCase(MinimalJiraDbContext dbContext, ILogger<UpdateTaskUseCase> logger) : IUpdateTaskUseCase
{
    public async System.Threading.Tasks.Task ExecuteAsync(UpdateTaskCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Началось обновление задачи с Id: {Id}", command.Id);
        
        if (!await dbContext.Projects.AnyAsync(p => p.Id == command.ProjectId, cancellationToken))
        {
            logger.LogInformation("Не удалось найти проект с Id: {Id}", command.ProjectId);
            throw new NotFoundException($"{nameof(Domain.Entities.Project)} c Id: {command.ProjectId} не найден!");
        }

        var updateResult = await dbContext.Tasks
            .Where(t => t.Id == command.Id)
            .ExecuteUpdateAsync(setters => setters
                    .SetProperty(t => t.Title, command.Title)
                    .SetProperty(t => t.Description, command.Description)
                    .SetProperty(t => t.IsComplete, command.IsComplete)
                    .SetProperty(t => t.ProjectId, command.ProjectId)
                    .SetProperty(t => t.UpdatedAt, t => t.Title != command.Title
                                                        || t.Description != command.Description
                                                        || t.IsComplete != command.IsComplete
                                                        || t.ProjectId != command.ProjectId
                        ? DateTime.UtcNow
                        : t.UpdatedAt),
                cancellationToken);

        if (updateResult == 0)
        {
            logger.LogInformation("Не удалось найти задачу Id: {Id}", command.Id);
            throw new NotFoundException($"{nameof(Domain.Entities.Task)} с Id: {command.Id} не найден!");
        }
        
        logger.LogInformation("Задача с Id: {Id} успешно обновлена", command.Id);
    }
}