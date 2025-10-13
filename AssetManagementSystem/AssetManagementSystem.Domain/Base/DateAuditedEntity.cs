using AssetManagementSystem.Domain.Interface;

namespace AssetManagementSystem.Domain.Base;

/// <summary>
/// base class with added and modified date
/// </summary>
public abstract class DateAuditedEntity<TPrimaryKey> : Entity<TPrimaryKey>, IDateAuditedEntity
{
    public DateTime? AddedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}
