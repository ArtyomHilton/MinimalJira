using Microsoft.EntityFrameworkCore;
using MinimalJira.Domain.Exceptions;
using MinimalJira.Persistence.Context;

namespace MinimalJira.Application.UseCases.Task.DeleteTask;

public class DeleteTaskUseCase(MinimalJiraDbContext dbContext) : IDeleteTaskUseCase
{
    public async System.Threading.Tasks.Task ExecuteAsync(DeleteTaskCommand command, CancellationToken cancellationToken)
    {
        var deleteResult = await dbContext.Tasks.Where(t => t.Id == command.Id)
            .ExecuteDeleteAsync(cancellationToken);

        if (deleteResult == 0)
        {
            throw new NotFoundException($"{nameof(Domain.Entities.Task)} с Id: {command.Id} не найдена!");
        }
    }
}