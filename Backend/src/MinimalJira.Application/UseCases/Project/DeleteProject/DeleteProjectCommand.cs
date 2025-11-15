namespace MinimalJira.Application.UseCases.Project.DeleteProject;

/// <summary>
/// Команда удаления проекта.
/// </summary>
/// <param name="Id">Идентификатор проекта.</param>
public record DeleteProjectCommand(Guid Id);