using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MinimalJira.Domain.Exceptions;
using MinimalJira.Persistence.Context;

namespace MinimalJira.Application.UseCases.Project.UpdateProject;

public class UpdateProjectUseCase(MinimalJiraDbContext dbContext, ILogger<UpdateProjectUseCase> logger) : IUpdateProjectUseCase
{
    public async System.Threading.Tasks.Task ExecuteAsync(UpdateProjectCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Началось обновление проекта с Id: {Id}", command.Id);
        
        var updateResult = await dbContext.Projects.Where(p => p.Id == command.Id)
            .ExecuteUpdateAsync(setters => setters.SetProperty(p => p.Name, command.Name)
                .SetProperty(p => p.Description, command.Description)
                .SetProperty(p => p.UpdatedAt, p =>
                    p.Name != command.Name || p.Description != command.Description
                        ? DateTime.UtcNow
                        : p.UpdatedAt), 
                cancellationToken);

        if (updateResult == 0)
        {
            logger.LogInformation("Не удалось найти проект с Id: {Id}", command.Id);
            throw new NotFoundException($"{nameof(Domain.Entities.Project)} с Id: {command.Id} не найден!");
        }
        
        logger.LogInformation("Проект с Id: {Id} успешно обновлен", command.Id);
    }
}