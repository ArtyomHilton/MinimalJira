using MinimalJira.Application.DTOs;

namespace MinimalJira.Application.UseCases.Task.GetTaskById;

public interface IGetTaskByIdUseCase
{
    Task<TaskDataResponse> ExecuteAsync(GetTaskByIdQuery query, CancellationToken cancellationToken);
}