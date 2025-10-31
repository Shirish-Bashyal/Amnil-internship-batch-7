using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementSystem.Shared.Dtos.Asset;

public class AssetCountDto
{
    public Guid DepartmentId { get; set; }
   public string DepartmentName { get; set; }
    public int AssetCount { get; set; }

    public List<BasicAssetDto> Assets { get; set; }
}
