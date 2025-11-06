using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementSystem.Entity.Entities;

public class User:BaseModel
{
    public string UserName { get; set; }

    public string PasswordHashed { get; set; }

    public string Email { get; set; }


    public string Phone { get; set; }


    public Guid? RoleID { get; set; }


    [ForeignKey("RoleID")]
    public virtual Role Role { get; set; }

    public string? RefereshToken { get; set; }

    public DateTime? RefereshTokenExpiry { get; set; }

}
