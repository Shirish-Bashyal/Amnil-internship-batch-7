using AssetManagementSystem.Domain.Entities.Locations;

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

    public string? User { get; set; }

    public string? Building { get; set; }
    public string? Floor { get; set; }
    public string? Room { get; set; }

    public Guid? CategoryId { get; set; }
    public string? Category { get; set; } = string.Empty;

    public Guid? DepartmentId { get; set; }
    public string? Department { get; set; }

    public List<string> TagMacAddress { get; set; } = [];

    public byte[]? ImageData { get; set; }
}
