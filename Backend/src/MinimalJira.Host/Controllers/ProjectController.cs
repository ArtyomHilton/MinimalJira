using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MinimalJira.Application.UseCases.Project.AddProject;
using MinimalJira.Application.UseCases.Project.DeleteProject;
using MinimalJira.Application.UseCases.Project.GetProjectById;
using MinimalJira.Application.UseCases.Project.GetProjects;
using MinimalJira.Application.UseCases.Project.UpdateProject;
using MinimalJira.Host.DTOs.Request;
using MinimalJira.Host.DTOs.Response;
using MinimalJira.Host.Mappers;

namespace MinimalJira.Host.Controllers;

[ApiController]
[Route("/api/projects/")]
public class ProjectController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddProject([FromBody] AddProjectRequest request,
        [FromServices] IAddProjectUseCase useCase,
        [FromServices] IValidator<AddProjectRequest> validator,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.ToResponse());
        }
        
        var projectId = await useCase.ExecuteAsync(request.ToCommand(), cancellationToken);

        return CreatedAtAction(nameof(GetProject), new { id = projectId }, new { id = projectId });
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(ICollection<ProjectResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProjects([FromQuery] Pagination pagination,
        [FromServices] IGetProjectsUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.ExecuteAsync(pagination.ToQuery(), cancellationToken);

        return Ok(result.ToResponse());
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProject([FromRoute] Guid id,
        [FromServices] IGetProjectByIdUseCase useCase,
        CancellationToken cancellationToken)
    {
        var response = await useCase.ExecuteAsync(ProjectMappers.ToQuery(id), cancellationToken);

        return Ok(response.ToResponse());
    }
    
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateProject([FromRoute] Guid id, 
        [FromBody] UpdateProjectRequest request,
        [FromServices] IUpdateProjectUseCase useCase,
        [FromServices] IValidator<UpdateProjectRequest> validator,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.ToResponse());
        }
        
        await useCase.ExecuteAsync(request.ToCommand(id), cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteProject([FromRoute] Guid id,
        [FromServices] IDeleteProjectUseCase useCase,
        CancellationToken cancellationToken)
    {
        await useCase.ExecuteAsync(ProjectMappers.ToCommand(id), cancellationToken);

        return NoContent();
    }
}