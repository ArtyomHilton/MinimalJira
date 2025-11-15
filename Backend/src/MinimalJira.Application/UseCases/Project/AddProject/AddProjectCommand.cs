namespace MinimalJira.Application.UseCases.Project.AddProject;

/// <summary>
/// Команда добавления проекта.
/// </summary>
/// <param name="Name">Название проекта.</param>
/// <param name="Description">Описание проекта.</param>
public record AddProjectCommand(string Name, string Description);