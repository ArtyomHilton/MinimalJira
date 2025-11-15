using MinimalJira.Application.DTOs;

namespace MinimalJira.Application.UseCases.Project.GetProjectById;

public interface IGetProjectByIdUseCase
{
    Task<ProjectDataResponse> Execute(GetProjectByIdQuery query, CancellationToken cancellationToken);
}