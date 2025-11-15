using MinimalJira.Application.DTOs;

namespace MinimalJira.Application.UseCases.Project.GetProjects;

public interface IGetProjectsUseCase
{
    Task<ICollection<ProjectDataResponse>> Execute(GetProjectsQuery query, CancellationToken cancellationToken);
}