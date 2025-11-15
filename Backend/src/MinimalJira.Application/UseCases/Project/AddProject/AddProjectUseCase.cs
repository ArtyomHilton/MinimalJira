using MinimalJira.Application.Mappers;
using MinimalJira.Persistence.Context;

namespace MinimalJira.Application.UseCases.Project.AddProject;

public class AddProjectUseCase(MinimalJiraDbContext dbContext) : IAddProjectUseCase
{
    public async Task<Guid> Execute(AddProjectCommand command, CancellationToken cancellationToken)
    {
        var project = command.ToEntity();
        
        await dbContext.Projects.AddAsync(project, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return project.Id;
    }
}