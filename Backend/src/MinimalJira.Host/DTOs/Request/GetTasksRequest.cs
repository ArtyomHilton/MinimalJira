namespace MinimalJira.Host.DTOs.Request;

public record GetTasksRequest(bool? IsCompleted, Guid ProjectId);