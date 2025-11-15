namespace MinimalJira.Application.UseCases.Project.UpdateProject;

public interface IUpdateProjectUseCase
{
    Task Execute(UpdateProjectCommand command, CancellationToken cancellationToken);
}