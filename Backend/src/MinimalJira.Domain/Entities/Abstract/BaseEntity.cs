namespace MinimalJira.Domain.Entities.Abstract;

/// <summary>
/// Базовая сущность.
/// </summary>
public class BaseEntity
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Дата создания.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Дата обновления.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}