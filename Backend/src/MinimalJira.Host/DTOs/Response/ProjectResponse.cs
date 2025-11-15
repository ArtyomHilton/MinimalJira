namespace MinimalJira.Host.DTOs.Response;

/// <summary>
/// DTO с данными проекта.
/// </summary>
/// <param name="Id">Идентификатор проекта.</param>
/// <param name="Name">Название проекта.</param>
/// <param name="Description">Описание проекта.</param>
/// <param name="CreatedAt">Дата создания.</param>
/// <param name="Tasks">Список задач.</param>
public record ProjectResponse(
    Guid Id,
    string Name,
    string Description,
    DateTime CreatedAt,
    ICollection<TaskResponse> Tasks);