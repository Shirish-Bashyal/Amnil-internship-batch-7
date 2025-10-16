namespace AssetManagement.Domain.Dtos.AssetDto;

public class GetAssetDto
{
    public string? Name { get; set; }
    public string? SerialNumber { get; set; }
    public decimal Cost { get; set; }
    public string? UserName { get; set; }
    public bool? Status { get; set; }
    public string? DepartmentId { get; set; }
    public Guid CategoryId { get; set; }

    public string? DepartmentName { get; set; }
    public string? CategoryName { get; set; }
}
