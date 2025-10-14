using AssetManagement.Domain.Entity.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Domain.Entity.Application;

public class Tag:BaseEntity<Guid>
{
    public string? TagId { get; set; }
    public bool? IsActive { get; set; }
    public Asset? Asset { get; set; }
}
