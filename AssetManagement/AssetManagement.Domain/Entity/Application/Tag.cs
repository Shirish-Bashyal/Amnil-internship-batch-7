using AssetManagement.Domain.Entity.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Domain.Entity.Application;

public class Tag:Audit<Guid>
{
    public string TagId { get; set; } = string.Empty;
    public bool IsActive { get; set; }=false;
    public Asset? Asset { get; set; }
}
