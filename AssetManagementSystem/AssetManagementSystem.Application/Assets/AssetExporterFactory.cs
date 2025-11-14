using AssetManagementSystem.Contracts.Assets;

namespace AssetManagementSystem.Application.Assets;

/// <summary>
///
/// </summary>
public class AssetExporterFactory
{
    public static IAssetExporter GetExporterAsync(string format)
    {
        return format?.ToLower() switch
        {
            "excel" => new ExcelAssetExporter(),
            "pdf" => new PdfAssetExporter(),
            _ => new ExcelAssetExporter(),
        };
    }
}
