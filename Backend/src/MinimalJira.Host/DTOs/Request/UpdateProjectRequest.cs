namespace MinimalJira.Host.DTOs.Request;

/// <summary>
/// Команда обновления проекта.
/// </summary>
/// <param name="Name">Имя проекта.</param>
/// <param name="Description">Описание проекта</param>
public record UpdateProjectRequest(string Name, string Description);