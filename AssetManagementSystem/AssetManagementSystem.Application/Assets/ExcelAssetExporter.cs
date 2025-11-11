using AssetManagementSystem.Contracts.Assets;
using ClosedXML.Excel;

namespace AssetManagementSystem.Application.Assets;

/// <summary>
///
/// </summary>
public class ExcelAssetExporter : IAssetExporter
{
    public string ContentType =>
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    public string FileName => "Assets.xlsx";

    public byte[] Export(List<AssetDto> assets)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Assets");

        worksheet.Cell(1, 2).Value = "Asset Name";
        worksheet.Cell(1, 3).Value = "Serial Number";
        worksheet.Cell(1, 4).Value = "Description";
        worksheet.Cell(1, 5).Value = "Is Active";
        worksheet.Cell(1, 6).Value = "Received Date";

        worksheet.Cell(1, 8).Value = "Category Name";

        worksheet.Cell(1, 10).Value = "Department Name";

        var dataToInsert = assets.Select(a =>
            new object[]
            {
                a.Name,
                a.SerialNumber,
                a.Description ?? string.Empty,
                a.IsActive,
                a.ReceivedDate,
                a.Category ?? string.Empty,
                a.Department ?? string.Empty
            }
        );

        worksheet.Cell(2, 1).InsertData(dataToInsert);
        worksheet.Columns().AdjustToContents();
        worksheet.Column(5).Style.DateFormat.SetFormat("yyyy-MM-dd");

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
