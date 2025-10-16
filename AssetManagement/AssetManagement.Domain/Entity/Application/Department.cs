using AssetManagement.Domain.Entity.BaseEntity;

namespace AssetManagement.Domain.Entity.Application;

public class Department : Audit<Guid>
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public ICollection<Asset>? Asset { get; set; }
    public ICollection<User>? User { get; set; }
}
