using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MinimalJira.Application.DTOs;
using MinimalJira.Domain.Exceptions;
using MinimalJira.Persistence.Context;

namespace MinimalJira.Application.UseCases.Task.GetTaskById;

public class GetTaskByIdUseCase(MinimalJiraDbContext dbContext, ILogger<GetTaskByIdUseCase> logger)
    : IGetTaskByIdUseCase
{
    public async Task<TaskDataResponse> ExecuteAsync(GetTaskByIdQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Началось получение задачи с Id: {Id}", query.TaskId);

        var task = await dbContext.Tasks.Select(t => new TaskDataResponse
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                CreatedAt = t.CreatedAt,
                IsComplete = t.IsComplete
            }).AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == query.TaskId, cancellationToken);

        if (task is null)
        {
            logger.LogInformation("Не удалось найти задачу с Id: {Id}", query.TaskId);
            throw new NotFoundException($"{nameof(Domain.Entities.Task)} с Id: {query.TaskId} не найдена!");
        }

        logger.LogInformation("Задача с Id: {Id} успешно найдена", query.TaskId);

        return task;
    }
}