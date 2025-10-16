using AssetManagement.Domain.Entity.BaseEntity;

namespace AssetManagement.Domain.Entity.Application;

public class Category : Audit<Guid>
{
    public string CategoryName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ICollection<Asset>? Assets { get; set; }
}
