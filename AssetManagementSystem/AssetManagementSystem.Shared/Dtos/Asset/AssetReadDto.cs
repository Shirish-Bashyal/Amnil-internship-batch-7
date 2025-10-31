using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementSystem.Shared.Dtos.Asset;

public class AssetReadDto:BaseDto
{
    public Guid Id { get; set; }
    public string AssetName { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public string AssetCategory { get; set; } = string.Empty;
    public DateTime ReceivedDate { get; set; }
    public bool IsActive { get; set; }
    public Guid DepartmentId { get; set; }

    public bool IsActivated { get; set; }
    public string? DepartmentName { get; set; } // optional: from navigation property

    
    public Guid? TagId { get; set; }
    public string? TagName { get; set; } // optional: from navigation property
}
