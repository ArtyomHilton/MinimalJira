namespace MinimalJira.Application.UseCases.Task.AddTask;

/// <summary>
/// Команда добавления задачи.
/// </summary>
/// <param name="Title">Название.</param>
/// <param name="Description">Описание.</param>
/// <param name="IsComplete">Статус выполнения.</param>
/// <param name="ProjectId">Идентификатор проекта.</param>
public record AddTaskCommand(string Title, string Description, bool IsComplete, Guid ProjectId);