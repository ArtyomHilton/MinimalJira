using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MinimalJira.Domain.Exceptions;
using MinimalJira.Persistence.Context;

namespace MinimalJira.Application.UseCases.Task.DeleteTask;

public class DeleteTaskUseCase(MinimalJiraDbContext dbContext, ILogger<DeleteTaskUseCase> logger) : IDeleteTaskUseCase
{
    public async System.Threading.Tasks.Task ExecuteAsync(DeleteTaskCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Началось удаление задачи с Id: {Id}", command.Id);
        
        var deleteResult = await dbContext.Tasks.Where(t => t.Id == command.Id)
            .ExecuteDeleteAsync(cancellationToken);

        if (deleteResult == 0)
        {
            logger.LogInformation("Не удалось найти задачу с Id: {Id}", command.Id);
            throw new NotFoundException($"{nameof(Domain.Entities.Task)} с Id: {command.Id} не найдена!");
        }
        
        logger.LogInformation("Задача с Id: {Id} успешно удалена", command.Id);
    }
}