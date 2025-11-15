using Microsoft.EntityFrameworkCore;
using MinimalJira.Domain.Exceptions;
using MinimalJira.Persistence.Context;

namespace MinimalJira.Application.UseCases.Project.DeleteProject;

public class DeleteProjectUseCase(MinimalJiraDbContext dbContext) : IDeleteProjectUseCase
{
    public async Task Execute(DeleteProjectCommand command, CancellationToken cancellationToken)
    {
        var deleteResult = await dbContext.Projects
            .Where(p => p.Id == command.Id)
            .ExecuteDeleteAsync(cancellationToken);

        if (deleteResult == 0)
        {
            throw new NotFoundException($"Проект с Id: {command.Id} не найден!");
        }
    }
}