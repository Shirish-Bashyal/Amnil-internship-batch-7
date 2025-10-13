using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Domain.Entity.BaseEntity;

public abstract class BaseEntity<T>
{
    public required T Id { get; set; }
}
