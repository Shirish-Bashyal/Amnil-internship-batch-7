namespace AssetManagementSystem.Domain.Interface;

/// <summary>
///Defines properties to track creation and modification date
/// </summary>
public interface IDateAuditedEntity
{
    public DateTime? AddedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}
