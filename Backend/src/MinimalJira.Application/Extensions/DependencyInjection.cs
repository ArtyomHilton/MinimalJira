using Microsoft.Extensions.DependencyInjection;
using MinimalJira.Application.UseCases.Project.AddProject;
using MinimalJira.Application.UseCases.Project.DeleteProject;
using MinimalJira.Application.UseCases.Project.GetProjectById;
using MinimalJira.Application.UseCases.Project.GetProjects;
using MinimalJira.Application.UseCases.Project.UpdateProject;

namespace MinimalJira.Application.Extensions;

/// <summary>
/// Внедрение зависимостей.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Регистрирует UseCases в DI.
    /// </summary>
    /// <param name="serviceCollection"><see cref="IServiceCollection"/></param>
    /// <returns>Модифицированный <paramref name="serviceCollection"/></returns>
    public static IServiceCollection AddUseCases(this IServiceCollection serviceCollection) =>
        serviceCollection.AddScoped<IAddProjectUseCase, AddProjectUseCase>()
            .AddScoped<IGetProjectsUseCase, GetProjectsUseCase>()
            .AddScoped<IGetProjectByIdUseCase, GetProjectByIdUseCase>()
            .AddScoped<IUpdateProjectUseCase, UpdateProjectUseCase>()
            .AddScoped<IDeleteProjectUseCase, DeleteProjectUseCase>();
}