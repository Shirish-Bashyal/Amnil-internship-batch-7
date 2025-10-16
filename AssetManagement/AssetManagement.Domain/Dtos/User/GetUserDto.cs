using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Domain.Dtos.User;

public class GetUserDto
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public Guid DepartmentId { get; set; }
}
