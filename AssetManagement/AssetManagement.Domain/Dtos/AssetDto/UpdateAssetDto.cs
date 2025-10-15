namespace AssetManagement.Domain.Dtos.AssetDto;

public class UpdateAssetDto
{
    public string? Name { get; set; }
    public decimal Cost { get; set; }
    public Guid? UserId { get; set; }
    public Guid? DepartmentId { get; set; }
    public bool? Status { get; set; }
}
