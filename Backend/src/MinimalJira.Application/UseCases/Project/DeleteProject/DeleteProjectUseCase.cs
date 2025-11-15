using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MinimalJira.Domain.Exceptions;
using MinimalJira.Persistence.Context;

namespace MinimalJira.Application.UseCases.Project.DeleteProject;

public class DeleteProjectUseCase(MinimalJiraDbContext dbContext, ILogger<DeleteProjectUseCase> logger) : IDeleteProjectUseCase
{
    public async System.Threading.Tasks.Task ExecuteAsync(DeleteProjectCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Началось удаление проекта с Id: {Id}", command.Id);
        
        var deleteResult = await dbContext.Projects
            .Where(p => p.Id == command.Id)
            .ExecuteDeleteAsync(cancellationToken);

        if (deleteResult == 0)
        {
            logger.LogInformation("Не удалось найти проект с Id: {Id}", command.Id);
            throw new NotFoundException($"Проект с Id: {command.Id} не найден!");
        }
        
        logger.LogInformation("Проект с Id: {Id} успешно удален", command.Id);
    }
}