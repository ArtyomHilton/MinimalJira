using Microsoft.EntityFrameworkCore;
using MinimalJira.Application.DTOs;
using MinimalJira.Domain.Exceptions;
using MinimalJira.Persistence.Context;

namespace MinimalJira.Application.UseCases.Task.GetTaskById;

public class GetTaskByIdUseCase(MinimalJiraDbContext dbContext) : IGetTaskByIdUseCase
{
    public async Task<TaskDataResponse> ExecuteAsync(GetTaskByIdQuery query, CancellationToken cancellationToken)
    {
        var task = await dbContext.Tasks.Select(t => new TaskDataResponse
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            CreatedAt = t.CreatedAt,
            IsComplete = t.IsComplete
        }).FirstOrDefaultAsync(t => t.Id == query.TaskId, cancellationToken);

        return task ?? throw new NotFoundException($"{nameof(Domain.Entities.Task)} с Id: {query.TaskId} не найдена!");
    }
}