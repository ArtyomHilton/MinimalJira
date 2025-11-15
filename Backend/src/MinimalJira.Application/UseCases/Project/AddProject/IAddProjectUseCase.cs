namespace MinimalJira.Application.UseCases.Project.AddProject;

public interface IAddProjectUseCase
{
    Task<Guid> Execute(AddProjectCommand command, CancellationToken cancellationToken);
}