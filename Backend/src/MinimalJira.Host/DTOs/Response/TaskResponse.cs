namespace MinimalJira.Host.DTOs.Response;

/// <summary>
/// DTO с данными задачи.
/// </summary>
/// <param name="Id">Идентификатор задачи.</param>
/// <param name="Title">Название задачи.</param>
/// <param name="Description">Описание задачи.</param>
/// <param name="CreatedAt">Дата создания.</param>
/// <param name="IsComplete">Статус задачи.</param>
public record TaskResponse(Guid Id, string Title, string Description, DateTime CreatedAt, bool IsComplete);