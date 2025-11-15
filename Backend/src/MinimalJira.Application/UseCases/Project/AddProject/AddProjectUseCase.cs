using Microsoft.Extensions.Logging;
using MinimalJira.Application.Mappers;
using MinimalJira.Persistence.Context;

namespace MinimalJira.Application.UseCases.Project.AddProject;

public class AddProjectUseCase(MinimalJiraDbContext dbContext, ILogger<AddProjectUseCase> logger) : IAddProjectUseCase
{
    public async Task<Guid> ExecuteAsync(AddProjectCommand command, CancellationToken cancellationToken)
    {
        var project = command.ToEntity();
        
        logger.LogInformation("Началось создание проекта с Id: {Id}", project.Id);
        
        await dbContext.Projects.AddAsync(project, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
        
        logger.LogInformation("Проект с Id: {Id} успешно создан", project.Id);

        return project.Id;
    }
}