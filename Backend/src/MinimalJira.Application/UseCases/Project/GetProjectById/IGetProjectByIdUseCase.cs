using MinimalJira.Application.DTOs;

namespace MinimalJira.Application.UseCases.Project.GetProjectById;

public interface IGetProjectByIdUseCase
{
    Task<ProjectDataResponse> ExecuteAsync(GetProjectByIdQuery query, CancellationToken cancellationToken);
}