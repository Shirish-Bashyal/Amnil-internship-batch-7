namespace AssetManagementSystem.Client.Models.Assets;

/// <summary>
/// represents details of asset shown in frontend
/// </summary>
public class AssetViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime? ReceivedDate { get; set; }
    public bool IsActive { get; set; }

    public Guid CategoryId { get; set; }
    public string? Category { get; set; } = string.Empty;

    public Guid? DepartmentId { get; set; }
    public string? Department { get; set; }
    public List<string> TagMacAddress { get; set; } = [];
    public byte[]? ImageData { get; set; }

    public string? ImageDataUri =>
        ImageData == null ? null : $"data:image/png;base64,{Convert.ToBase64String(ImageData)}";
}
