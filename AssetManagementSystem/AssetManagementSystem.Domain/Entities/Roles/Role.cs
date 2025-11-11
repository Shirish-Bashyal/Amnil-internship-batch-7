using AssetManagementSystem.Domain.Base;
using AssetManagementSystem.Domain.Entities.Users;

namespace AssetManagementSystem.Domain.Entities.Roles;

/// <summary>
/// represents roles in application
/// </summary>
public class Role : DateAuditedEntity<Guid>
{
    public string Name { get; set; } = string.Empty;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
