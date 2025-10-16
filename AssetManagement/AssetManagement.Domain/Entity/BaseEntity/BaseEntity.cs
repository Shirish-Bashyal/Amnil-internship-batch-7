namespace AssetManagement.Domain.Entity.BaseEntity;

public abstract class BaseEntity<T>
{
    public required T Id { get; set; }
}
