using AssetManagementSystem.Shared.Constrains;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementSystem.Shared.Dtos.Asset;

public class AssetDto
{
    public Guid Id { get; set; } 

    [Required]
    public string AssetName { get; set; } = string.Empty;

    [Required]
    public string SerialNumber { get; set; } = string.Empty;

    public bool IsActivated { get; set; }


    [Required]
    public string AssetCategory { get; set; } = string.Empty;

    [Required]
    public DateTime ReceivedDate { get; set; }

    [Required]
    public Guid DepartmentId { get; set; }

    public Guid? TagId { get; set; }

    public string? Description { get; set; }
}
