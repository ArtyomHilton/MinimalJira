namespace MinimalJira.Application.UseCases.Project.DeleteProject;

public interface IDeleteProjectUseCase
{
    System.Threading.Tasks.Task ExecuteAsync(DeleteProjectCommand command, CancellationToken cancellationToken);
}