namespace MinimalJira.Application.UseCases.Task.UpdateTask;

public interface IUpdateTaskUseCase
{
    System.Threading.Tasks.Task ExecuteAsync(UpdateTaskCommand command, CancellationToken cancellationToken);
}