using AssetManagement.Domain.Entity.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Domain.Entity.Application;

public class Department:Audit<Guid>
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public ICollection<Asset>? Asset { get; set; }
    public ICollection<User>? User { get; set; }
}
