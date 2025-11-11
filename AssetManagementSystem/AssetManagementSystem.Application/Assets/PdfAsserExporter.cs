using AssetManagementSystem.Contracts.Assets;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace AssetManagementSystem.Application.Assets;

/// <summary>
///
/// </summary>
public class PdfAssetExporter : IAssetExporter
{
    public string ContentType => "application/pdf";
    public string FileName => "Assets.pdf";

    public byte[] Export(List<AssetDto> assets)
    {
        var stream = new MemoryStream();

        Document
            .Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(20);
                    page.Size(PageSizes.A4);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Text("Assets Report").SemiBold().FontSize(16).AlignCenter();

                    page.Content()
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2); // Name
                                columns.RelativeColumn(1.5f); // Serial
                                columns.RelativeColumn(2.5f); // Description (wraps)
                                columns.ConstantColumn(45); // IsActive
                                columns.ConstantColumn(70); // Received

                                columns.RelativeColumn(1.5f); // Cat Name

                                columns.RelativeColumn(1.5f); // Dept Name
                            });

                            // Table Header
                            table.Header(header =>
                            {
                                header.Cell().Text("Asset Name");
                                header.Cell().Text("Serial Number");
                                header.Cell().Text("Description");
                                header.Cell().Text("Is Active");
                                header.Cell().Text("Received Date");

                                header.Cell().Text("Category Name");

                                header.Cell().Text("Department Name");
                            });

                            // Table Rows
                            foreach (var a in assets)
                            {
                                table.Cell().Text(a.Name);
                                table.Cell().Text(a.SerialNumber);
                                table.Cell().Text(a.Description ?? string.Empty);
                                table.Cell().Text(a.IsActive ? "Yes" : "No");
                                table
                                    .Cell()
                                    .Text(
                                        a.ReceivedDate.HasValue
                                            ? a.ReceivedDate.Value.ToString("yyyy-MM-dd")
                                            : string.Empty
                                    );

                                table.Cell().Text(a.Category ?? string.Empty);

                                table.Cell().Text(a.Department ?? string.Empty);
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.CurrentPageNumber();
                            x.Span(" / ");
                            x.TotalPages();
                        });
                });
            })
            .GeneratePdf(stream);

        return stream.ToArray();
    }
}
