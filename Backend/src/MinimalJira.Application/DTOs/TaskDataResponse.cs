namespace MinimalJira.Application.DTOs;
public record TaskDataResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public bool IsComplete { get; set; }
}