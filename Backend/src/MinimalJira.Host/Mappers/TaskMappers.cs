using MinimalJira.Application.DTOs;
using MinimalJira.Application.UseCases.Task.AddTask;
using MinimalJira.Application.UseCases.Task.DeleteTask;
using MinimalJira.Application.UseCases.Task.GetTaskById;
using MinimalJira.Application.UseCases.Task.GetTasks;
using MinimalJira.Application.UseCases.Task.UpdateTask;
using MinimalJira.Host.DTOs.Request;
using MinimalJira.Host.DTOs.Response;

namespace MinimalJira.Host.Mappers;

public static class TaskMappers
{
    public static TaskResponse ToResponse(this TaskDataResponse response) =>
        new TaskResponse(response.Id, response.Title, response.Description, response.CreatedAt, response.IsComplete);

    public static ICollection<TaskResponse> ToResponse(this ICollection<TaskDataResponse> responses) =>
        responses.Select(tdr => tdr.ToResponse())
            .ToList();

    public static AddTaskCommand ToCommand(this AddTaskRequest request) =>
        new AddTaskCommand(request.Title, request.Description, request.IsComplete, request.ProjectId);

    public static GetTasksQuery ToQuery(this GetTasksRequest request) =>
        new GetTasksQuery(request.IsCompleted, request.ProjectId);

    public static GetTaskByIdQuery ToQuery(Guid id) =>
        new GetTaskByIdQuery(id);

    public static UpdateTaskCommand ToCommand(this UpdateTaskRequest request, Guid id) =>
        new UpdateTaskCommand(id, request.Title, request.Description, request.IsComplete, request.ProjectId);

    public static DeleteTaskCommand ToCommand(Guid id) =>
        new DeleteTaskCommand(id);
}