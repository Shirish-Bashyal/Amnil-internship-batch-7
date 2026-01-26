namespace AssetManagementSystem.Domain.Base;

/// <summary>
/// represents base class with id
/// </summary>
public abstract class Entity<TPrimaryKey>
{
    public TPrimaryKey Id { get; set; } = default!;
}
