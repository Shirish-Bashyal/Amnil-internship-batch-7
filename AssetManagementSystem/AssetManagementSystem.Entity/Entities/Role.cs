using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementSystem.Entity.Entities;

public class Role:BaseModel
{
    public string RoleName { get; set; }


    public ICollection<User> Users { get; set; }

    public static implicit operator Role?(User? v)
    {
        throw new NotImplementedException();
    }
}
