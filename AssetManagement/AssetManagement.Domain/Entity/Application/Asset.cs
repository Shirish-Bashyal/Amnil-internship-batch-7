using AssetManagement.Domain.Entity.BaseEntity;

namespace AssetManagement.Domain.Entity.Application;

public class Asset : Audit<Guid>
{
    public string Name { get; set; } = string.Empty;//description  data seed
    public string SerialNumber { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Cost { get; set; }
    public Guid? UserId { get; set; }
    public Guid TagId { get; set; }
    public Guid? DepartmentId { get; set; }
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
    public Tag? Tag { get; set; }
    public User? User { get; set; }
    public Department? Department { get; set; }
}
