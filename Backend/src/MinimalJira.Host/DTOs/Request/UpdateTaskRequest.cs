namespace MinimalJira.Host.DTOs.Request;

/// <summary>
/// Команда обновления задачи.
/// </summary>
/// <param name="Id">Идентификатор задачи.</param>
/// <param name="Title">Название.</param>
/// <param name="Description">Описание.</param>
/// <param name="IsComplete">Статус выполнения.</param>
/// <param name="ProjectId">Идентификатор проекта.</param>
public record UpdateTaskRequest(Guid Id, string Title, string Description, bool IsComplete, Guid ProjectId);