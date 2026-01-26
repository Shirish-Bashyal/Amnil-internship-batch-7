using System.ComponentModel.DataAnnotations.Schema;
using AssetManagementSystem.Domain.Base;
using AssetManagementSystem.Domain.Entities.Assets;

namespace AssetManagementSystem.Domain.Entities.Tags;

/// <summary>
///
/// </summary>
public class Tag : DateAuditedEntity<Guid>
{
    public string MacAddress { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public Guid? AssetId { get; set; }

    [ForeignKey("AssetId")]
    public virtual Asset? Asset { get; set; }
}
