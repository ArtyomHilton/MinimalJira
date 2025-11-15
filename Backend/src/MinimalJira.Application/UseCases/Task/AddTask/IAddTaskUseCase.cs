namespace MinimalJira.Application.UseCases.Task.AddTask;

public interface IAddTaskUseCase
{
    Task<Guid> ExecuteAsync(AddTaskCommand command, CancellationToken cancellationToken);
}