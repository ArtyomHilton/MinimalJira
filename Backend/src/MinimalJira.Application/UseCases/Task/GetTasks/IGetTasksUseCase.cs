using MinimalJira.Application.DTOs;

namespace MinimalJira.Application.UseCases.Task.GetTasks;

public interface IGetTasksUseCase
{
    Task<ICollection<TaskDataResponse>> ExecuteAsync(GetTasksQuery query, CancellationToken cancellationToken);
}