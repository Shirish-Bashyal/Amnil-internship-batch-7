using AssetManagement.Domain.Entity.BaseEntity;

namespace AssetManagement.Domain.Entity.Application;

public class User : Audit<Guid>
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public Guid DepartmentId { get; set; }
    public Department? Department { get; set; }
    public ICollection<Asset>? Assets { get; set; }
}
