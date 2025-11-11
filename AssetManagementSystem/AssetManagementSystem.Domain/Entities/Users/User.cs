using System.ComponentModel.DataAnnotations.Schema;
using AssetManagementSystem.Domain.Base;
using AssetManagementSystem.Domain.Entities.Assets;
using AssetManagementSystem.Domain.Entities.Departments;
using AssetManagementSystem.Domain.Entities.Roles;

namespace AssetManagementSystem.Domain.Entities.Users;

/// <summary>
///represents a user entity
/// </summary>
public class User : DateAuditedEntity<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public virtual ICollection<Asset> Assets { get; set; } = [];

    public Guid RoleId { get; set; }

    public virtual Role Role { get; set; } = null!;
}
