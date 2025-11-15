using Microsoft.EntityFrameworkCore;
using MinimalJira.Application.Mappers;
using MinimalJira.Domain.Exceptions;
using MinimalJira.Persistence.Context;

namespace MinimalJira.Application.UseCases.Task.AddTask;

public class AddTaskUseCase(MinimalJiraDbContext dbContext) : IAddTaskUseCase
{
    public async Task<Guid> ExecuteAsync(AddTaskCommand command, CancellationToken cancellationToken)
    {
        if (!await dbContext.Projects.AnyAsync(p => p.Id == command.ProjectId, cancellationToken))
        {
            throw new NotFoundException($"{nameof(Domain.Entities.Project)} c Id: {command.ProjectId} не найден!");
        }

        var task = command.ToEntity();

        await dbContext.Tasks.AddAsync(task, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return task.Id;
    }
}