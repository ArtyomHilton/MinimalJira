namespace MinimalJira.Application.UseCases.Task.GetTasks;

/// <summary>
/// Команда получения задач.
/// </summary>
/// <param name="IsComplete">Статус выполнения.</param>
/// <param name="ProjectId">Идентификатор проекта.</param>
public record GetTasksQuery(bool? IsComplete, Guid ProjectId);