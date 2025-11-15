using Microsoft.EntityFrameworkCore;
using MinimalJira.Domain.Exceptions;
using MinimalJira.Persistence.Context;

namespace MinimalJira.Application.UseCases.Project.UpdateProject;

public class UpdateProjectUseCase(MinimalJiraDbContext dbContext) : IUpdateProjectUseCase
{
    public async Task Execute(UpdateProjectCommand command, CancellationToken cancellationToken)
    {
        
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
            throw new NotFoundException($"Проект с Id: {command.Id} не найден!");
        }
    }
}