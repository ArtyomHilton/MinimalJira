namespace MinimalJira.Application.UseCases.Project.DeleteProject;

public interface IDeleteProjectUseCase
{
    Task Execute(DeleteProjectCommand command, CancellationToken cancellationToken);
}