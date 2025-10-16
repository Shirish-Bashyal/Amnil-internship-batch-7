using AssetManagement.Domain.Entity.BaseEntity;

namespace AssetManagement.Domain.Entity.Application;

public class Tag
{
    public Guid TagId { get; set; }
    public string MacAddress { get; set; }=string.Empty;
    public bool IsActive { get; set; } = false;
    public Asset? Asset { get; set; }
}
