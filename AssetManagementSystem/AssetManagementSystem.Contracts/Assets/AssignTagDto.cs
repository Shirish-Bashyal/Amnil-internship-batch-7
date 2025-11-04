namespace AssetManagementSystem.Contracts.Assets;

public record AssignTagDto
{
    public Guid TagId { get; set; }

    public Guid AssetId { get; set; }
}
