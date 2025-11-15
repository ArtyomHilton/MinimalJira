using MinimalJira.Application.UseCases.Task.AddTask;
using Task = MinimalJira.Domain.Entities.Task;

namespace MinimalJira.Application.Mappers;

public static class TaskMappers
{
    public static Task ToEntity(this AddTaskCommand command) =>
        new Task()
        {
            Id = Guid.NewGuid(),
            Title = command.Title,
            Description = command.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsComplete = command.IsComplete,
            ProjectId = command.ProjectId
        };
}