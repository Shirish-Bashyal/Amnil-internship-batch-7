namespace AssetManagementSystem.Contracts.Tags;

/// <summary>
/// dto to return tag details
/// </summary>
public record TagDto
{
    public Guid Id { get; set; }
    public string MacAddress { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}
