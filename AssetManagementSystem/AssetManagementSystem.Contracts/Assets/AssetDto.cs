namespace AssetManagementSystem.Contracts.Assets;

/// <summary>
///Dto to return asset details
/// </summary>
public record AssetDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime? ReceivedDate { get; set; }
    public bool IsActive { get; set; }

    public Guid? CategoryId { get; set; }
    public string? Category { get; set; } = string.Empty;

    public Guid? DepartmentId { get; set; }
    public string? Department { get; set; }
}
