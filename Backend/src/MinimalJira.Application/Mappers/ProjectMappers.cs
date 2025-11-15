using MinimalJira.Application.UseCases.Project.AddProject;
using MinimalJira.Domain.Entities;

namespace MinimalJira.Application.Mappers;

public static class ProjectMappers
{
    public static Project ToEntity(this AddProjectCommand command) => new Project()
    {
        Id = Guid.NewGuid(),
        Name = command.Name,
        Description = command.Description,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    };
}