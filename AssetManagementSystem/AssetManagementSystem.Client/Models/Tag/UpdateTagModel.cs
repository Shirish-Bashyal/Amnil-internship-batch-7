namespace AssetManagementSystem.Client.Models.Tag;

/// <summary>
/// represents tag details to update
/// </summary>
public class UpdateTagModel
{
    public string MacAddress { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}
