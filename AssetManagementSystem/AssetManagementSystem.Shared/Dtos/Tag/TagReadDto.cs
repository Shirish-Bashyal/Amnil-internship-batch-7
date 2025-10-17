using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementSystem.Shared.Dtos.Tag;

public class TagReadDto
{
    public Guid Id { get; set; }
    public string MacAddress { get; set; } = string.Empty;
    public Guid? AssetId { get; set; }
    public string AssetName { get; set; } // optional
    public bool IsActive { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }
}
