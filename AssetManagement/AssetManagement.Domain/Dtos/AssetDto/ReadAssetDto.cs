using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Domain.Dtos.AssetDto;

public class ReadAssetDto
{
    public string? Name { get; set; }
    public string? SerialNumber { get; set; }
    public decimal Cost { get; set; }
    public Guid? UserName { get; set; }
    public bool Status {  get; set; }
    public string? DepartmentName { get; set; }
    public Guid CategoryId { get; set; }
}
