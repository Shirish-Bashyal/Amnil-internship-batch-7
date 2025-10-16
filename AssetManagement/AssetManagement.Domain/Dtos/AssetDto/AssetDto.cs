namespace AssetManagement.Domain.Dtos.AssetDto;

public class AssetDto
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? SerialNumber { get; set; }
    public DateTime? CreatedDate { get; set; }
    public decimal Cost { get; set; }
    public Guid? UserId { get; set; }
    public Guid? TagId { get; set; }
    public Guid? DepartmentId { get; set; }
    public Guid CategoryId { get; set; }
}
