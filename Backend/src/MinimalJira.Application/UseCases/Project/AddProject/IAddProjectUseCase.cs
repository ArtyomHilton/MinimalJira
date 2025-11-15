namespace MinimalJira.Application.UseCases.Project.AddProject;

public interface IAddProjectUseCase
{
    Task<Guid> ExecuteAsync(AddProjectCommand command, CancellationToken cancellationToken);
}