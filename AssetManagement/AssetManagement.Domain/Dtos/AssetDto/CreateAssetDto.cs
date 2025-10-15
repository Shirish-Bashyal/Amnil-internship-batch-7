using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Domain.Dtos.AssetDto;

public class CreateAssetDto
{
    public string? Name { get; set; }
    public string? SerialNumber { get; set; }
    public decimal Cost { get; set; }
    public Guid? UserId { get; set; }
    public Guid? TagId { get; set; }
    public Guid? DepartmentId { get; set; }
    public Guid CategoryId { get; set; }
}
