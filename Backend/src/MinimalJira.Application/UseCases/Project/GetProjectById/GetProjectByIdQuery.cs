namespace MinimalJira.Application.UseCases.Project.GetProjectById;

/// <summary>
/// Запрос получения проекта по его идентификатору.
/// </summary>
/// <param name="Id">Идентификатор проекта.</param>
public record GetProjectByIdQuery(Guid Id);