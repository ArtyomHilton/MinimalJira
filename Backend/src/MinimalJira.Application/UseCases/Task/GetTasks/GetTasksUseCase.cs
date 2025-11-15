using Microsoft.EntityFrameworkCore;
using MinimalJira.Application.DTOs;
using MinimalJira.Persistence.Context;

namespace MinimalJira.Application.UseCases.Task.GetTasks;

public class GetTasksUseCase(MinimalJiraDbContext dbContext) : IGetTasksUseCase
{
    public async Task<ICollection<TaskDataResponse>> ExecuteAsync(GetTasksQuery query, CancellationToken cancellationToken)
    {
        var queryTasks = dbContext.Tasks.AsQueryable();

        if (query.IsComplete is not null)
        {
            queryTasks = queryTasks.Where(t => t.IsComplete == query.IsComplete);
        }

        if (query.ProjectId != Guid.Empty)
        {
            queryTasks = queryTasks.Where(t => t.ProjectId == query.ProjectId);
        }

        var tasks = await queryTasks
            .Select(t=> new TaskDataResponse
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                CreatedAt = t.CreatedAt,
                IsComplete = t.IsComplete,
            })
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);

        return tasks;
    }
}