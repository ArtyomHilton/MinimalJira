using MinimalJira.Domain.Entities.Abstract;

namespace MinimalJira.Domain.Entities;

/// <summary>
/// Задача.
/// </summary>
public class Task : BaseEntity
{
    /// <summary>
    /// Название.
    /// </summary>
    public string Title { get; set; } = null!;
    
    /// <summary>
    /// Описание.
    /// </summary>
    public string Description { get; set; } = null!;
    
    /// <summary>
    /// Статус задачи.
    /// </summary>
    public bool IsComplete { get; set; }
    
    /// <summary>
    /// Идетификатор проекта.
    /// </summary>
    public Guid ProjectId { get; set; }
    
    /// <summary>
    /// Навигационное свойство к <see cref="Project"/>.
    /// </summary>
    public Project Project { get; set; } = null!;
}