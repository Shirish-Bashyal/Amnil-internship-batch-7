namespace AssetManagementSystem.Client.Models.Tag;

/// <summary>
/// represents tag details shown to frontend
/// </summary>
public class TagViewModel
{
    public Guid Id { get; set; }
    public string MacAddress { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}
