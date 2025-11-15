using MinimalJira.Application.DTOs;

namespace MinimalJira.Application.UseCases.Project.GetProjects;

public interface IGetProjectsUseCase
{
    Task<ICollection<ProjectDataResponse>> ExecuteAsync(GetProjectsQuery query, CancellationToken cancellationToken);
}