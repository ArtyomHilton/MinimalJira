using MinimalJira.Application.DTOs;
using MinimalJira.Host.DTOs.Response;

namespace MinimalJira.Host.Mappers;

public static class TaskMappers
{
    public static TaskResponse ToResponse(this TaskDataResponse response) =>
        new TaskResponse(response.Id, response.Title, response.Description, response.CreatedAt, response.IsComplete);

    public static ICollection<TaskResponse> ToResponse(this ICollection<TaskDataResponse> responses) =>
        responses.Select(tdr => tdr.ToResponse())
            .ToList();
}