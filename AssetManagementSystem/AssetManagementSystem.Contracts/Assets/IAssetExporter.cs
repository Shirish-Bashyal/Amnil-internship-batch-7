namespace AssetManagementSystem.Contracts.Assets;

/// <summary>
///defines operations for exporting assets data
/// </summary>
public interface IAssetExporter
{
    byte[] Export(List<AssetDto> assets);
    string ContentType { get; }
    string FileName { get; }
}
