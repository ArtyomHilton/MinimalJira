using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MinimalJira.Application.Mappers;
using MinimalJira.Domain.Exceptions;
using MinimalJira.Persistence.Context;

namespace MinimalJira.Application.UseCases.Task.AddTask;

public class AddTaskUseCase(MinimalJiraDbContext dbContext, ILogger<AddTaskUseCase> logger) : IAddTaskUseCase
{
    public async Task<Guid> ExecuteAsync(AddTaskCommand command, CancellationToken cancellationToken)
    {
        if (!await dbContext.Projects.AnyAsync(p => p.Id == command.ProjectId, cancellationToken))
        {
            logger.LogInformation("Не удалось найти проект с Id: {ProjectId}", command.ProjectId);
            throw new NotFoundException($"{nameof(Domain.Entities.Project)} c Id: {command.ProjectId} не найден!");
        }

        var task = command.ToEntity();

        logger.LogInformation("Началось создание задачи c Id: {TaskId} для проекта с Id: {ProjectId}", task.Id, task.ProjectId);
        
        await dbContext.Tasks.AddAsync(task, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Задача c Id: {TaskId} для проекта с Id: {ProjectId} успешно создана", task.Id, task.ProjectId);
        
        return task.Id;
    }
}