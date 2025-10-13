using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assisment.Entity.Entity;

public class ReportModel
{
    public string? InvoiceNumber { get; set; }

    public string? InvoiceName { get; set; }

    public List<ReportItem> ReportItem { get; set; }
}
