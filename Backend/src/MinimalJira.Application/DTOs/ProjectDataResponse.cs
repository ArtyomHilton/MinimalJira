namespace MinimalJira.Application.DTOs;

public record ProjectDataResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public ICollection<TaskDataResponse> Tasks { get; set; } = new List<TaskDataResponse>();
}