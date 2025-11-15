namespace MinimalJira.Application.UseCases.Task.GetTaskById;

/// <summary>
/// Команда получения задачи по идентификатору.
/// </summary>
/// <param name="TaskId">Идентификатор задачи.</param>
public record GetTaskByIdQuery(Guid TaskId);