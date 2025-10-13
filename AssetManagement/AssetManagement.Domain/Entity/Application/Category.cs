using AssetManagement.Domain.Entity.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Domain.Entity.Application;

public class Category:BaseEntity<Guid>
{
    public string? CategoryName { get; set; }
    public ICollection<Asset>? Assets { get; set; }
}
