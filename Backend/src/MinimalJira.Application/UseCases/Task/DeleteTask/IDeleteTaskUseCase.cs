namespace MinimalJira.Application.UseCases.Task.DeleteTask;

public interface IDeleteTaskUseCase
{
    System.Threading.Tasks.Task ExecuteAsync(DeleteTaskCommand command, CancellationToken cancellationToken);
}