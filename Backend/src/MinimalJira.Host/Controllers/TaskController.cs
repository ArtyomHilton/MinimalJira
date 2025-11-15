using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MinimalJira.Application.UseCases.Task.AddTask;
using MinimalJira.Application.UseCases.Task.DeleteTask;
using MinimalJira.Application.UseCases.Task.GetTaskById;
using MinimalJira.Application.UseCases.Task.GetTasks;
using MinimalJira.Application.UseCases.Task.UpdateTask;
using MinimalJira.Host.DTOs.Request;
using MinimalJira.Host.DTOs.Response;
using MinimalJira.Host.Mappers;

namespace MinimalJira.Host.Controllers;

[ApiController]
[Route("/api/tasks/")]
public class TaskController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddTask([FromBody] AddTaskRequest request,
        [FromServices] IAddTaskUseCase useCase,
        [FromServices] IValidator<AddTaskRequest> validator,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.ToResponse());
        }

        var taskId = await useCase.ExecuteAsync(request.ToCommand(), cancellationToken);

        return CreatedAtAction(nameof(GetTask), new { id = taskId }, new { id = taskId });
    }

    [HttpGet]
    [ProducesResponseType(typeof(ICollection<TaskResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTasks([FromQuery] GetTasksRequest request,
        [FromServices] IGetTasksUseCase useCase,
        CancellationToken cancellationToken)
    {
        var tasks = await useCase.ExecuteAsync(request.ToQuery(), cancellationToken);

        return Ok(tasks.ToResponse());
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTask([FromRoute] Guid id,
        [FromServices] IGetTaskByIdUseCase useCase,
        CancellationToken cancellationToken)
    {
        var task = await useCase.ExecuteAsync(TaskMappers.ToQuery(id), cancellationToken);

        return Ok(task.ToResponse());
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateTask([FromRoute] Guid id,
        [FromBody] UpdateTaskRequest request,
        [FromServices] IUpdateTaskUseCase useCase,
        [FromServices] IValidator<UpdateTaskRequest> validator,
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
    public async Task<IActionResult> DeleteTask([FromRoute] Guid id,
        [FromServices] IDeleteTaskUseCase useCase,
        CancellationToken cancellationToken)
    {
        await useCase.ExecuteAsync(TaskMappers.ToCommand(id), cancellationToken);

        return NoContent();
    }
}