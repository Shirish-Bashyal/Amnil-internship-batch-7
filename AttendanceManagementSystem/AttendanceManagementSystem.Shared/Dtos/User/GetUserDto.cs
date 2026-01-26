using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagementSystem.Shared.Dtos.User;

public class GetUserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public DateTime? CreationAt { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
}
