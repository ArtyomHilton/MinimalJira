namespace MinimalJira.Host.DTOs.Request;

/// <summary>
/// DTO добавления проекта.
/// </summary>
/// <param name="Name">Название проекта.</param>
/// <param name="Description">Описание проекта.</param>
public record AddProjectRequest(string Name, string Description);