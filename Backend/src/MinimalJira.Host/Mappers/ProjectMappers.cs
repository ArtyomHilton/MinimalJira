using MinimalJira.Application.DTOs;
using MinimalJira.Application.UseCases.Project.AddProject;
using MinimalJira.Application.UseCases.Project.DeleteProject;
using MinimalJira.Application.UseCases.Project.GetProjectById;
using MinimalJira.Application.UseCases.Project.GetProjects;
using MinimalJira.Application.UseCases.Project.UpdateProject;
using MinimalJira.Host.DTOs.Request;
using MinimalJira.Host.DTOs.Response;

namespace MinimalJira.Host.Mappers;

public static class ProjectMappers
{
    public static GetProjectsQuery ToQuery(this Pagination pagination) =>
        new GetProjectsQuery(pagination.Page, pagination.PageSize);

    public static ProjectResponse ToResponse(this ProjectDataResponse response) =>
        new ProjectResponse(response.Id, response.Name, response.Description, response.CreatedAt, response.Tasks.ToResponse());

    public static ICollection<ProjectResponse> ToResponse(this ICollection<ProjectDataResponse> responses) =>
        responses.Select(pdr => pdr.ToResponse())
            .ToList();

    public static AddProjectCommand ToCommand(this AddProjectRequest request) =>
        new AddProjectCommand(request.Name, request.Description);

    public static GetProjectByIdQuery ToQuery(Guid id) =>
        new GetProjectByIdQuery(id);

    public static UpdateProjectCommand ToCommand(this UpdateProjectRequest request, Guid id) =>
        new UpdateProjectCommand(id, request.Name, request.Description);

    public static DeleteProjectCommand ToCommand(Guid id) =>
        new DeleteProjectCommand(id);
}