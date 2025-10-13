using Assisment.Contract;
using Assisment.Contract.DTOs;
using Assisment.Contract.Interface.Service;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Assisment.Entity.Entity;
using QuestPDF.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Assisment.Application.Service;

public  class ReportRenderingService: IReportService
{
    public ReportRenderingService()
    {
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
    }

    // Accepts the ResponseData containing the list of students
    public async Task<byte[]> ReportRenderingAsync(ResponseData<List<StudentDTO>> data)
    {
       
        var students = data.Data ?? new List<StudentDTO>();

        
        var pdfBytes = QuestPDF.Fluent.Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);

                
                page.Header().Row(row =>
                {
                    row.RelativeItem()
                       .Column(column =>
                       {
                           column.Item().Text("Amnil Tech").FontSize(16).Bold();
                           column.Item().Text("Lalitpur").FontSize(12);
                       });

                    row.RelativeItem()
                       .ShowOnce()
                       .Text("Student Report")
                       .AlignRight()
                       .FontFamily("Arial")
                       .Bold()
                       .FontSize(20);
                });

                // Content
                page.Content()
                    .PaddingTop(20)
                    .Column(column =>
                    {
                        column.Item().Table(table =>
                        {
                            
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            // Header row
                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten2).Text("Name").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Text("Address").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Text("Email").Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Text("Gender").Bold();
                            });

                            // Data rows
                            foreach (var student in students)
                            {
                                table.Cell().Text(student.Name ?? "");
                                table.Cell().Text(student.Address ?? "");
                                table.Cell().Text(student.Email ?? "");
                                table.Cell().Text(student.Gender ?? "");
                            }
                        });
                    });

                
                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                        x.Span(" of ");
                        x.TotalPages();
                    });
            });
        }).GeneratePdf();

        return pdfBytes;
    }
}

