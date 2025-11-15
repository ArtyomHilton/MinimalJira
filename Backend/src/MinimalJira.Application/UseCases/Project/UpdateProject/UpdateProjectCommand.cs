namespace MinimalJira.Application.UseCases.Project.UpdateProject;

/// <summary>
/// Команда обновления проекта.
/// </summary>
/// <param name="Id">Идентификатор проекта.</param>
/// <param name="Name">Имя проекта.</param>
/// <param name="Description">Описание проекта</param>
public record UpdateProjectCommand(Guid Id, string Name, string Description);