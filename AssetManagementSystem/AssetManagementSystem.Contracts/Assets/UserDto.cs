namespace AssetManagementSystem.Contracts.Assets;

public record UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
