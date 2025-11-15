namespace MinimalJira.Application.UseCases.Project.UpdateProject;

public interface IUpdateProjectUseCase
{
    System.Threading.Tasks.Task ExecuteAsync(UpdateProjectCommand command, CancellationToken cancellationToken);
}