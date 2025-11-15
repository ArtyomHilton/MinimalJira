namespace MinimalJira.Application.UseCases.Task.DeleteTask;

/// <summary>
/// Команда удаления задачи.
/// </summary>
/// <param name="Id">Идентификатор задачи.</param>
public record DeleteTaskCommand(Guid Id);