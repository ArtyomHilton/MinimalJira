using MinimalJira.Domain.Entities.Abstract;
using Task = MinimalJira.Domain.Entities.Task;

namespace MinimalJira.Domain.Entities;

public class Project : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ICollection<Task> Tasks = new List<Task>();
}