using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assisment.Entity.Entity;

public class ReportItem
{
    
    public string Name { get; set; } = string.Empty;

    public string Gender { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;

    public string? Address { get; set; }
}
