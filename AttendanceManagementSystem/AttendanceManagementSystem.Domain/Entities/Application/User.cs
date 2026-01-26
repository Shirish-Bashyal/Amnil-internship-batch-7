using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttendanceManagementSystem.Domain.Entities.Base;

namespace AttendanceManagementSystem.Domain.Entities.Application;

/// <summary>
/// Represents a user in the attendance management system
/// </summary>
public class User : DateAuditedEntity<Guid>
{
    public string Name { get; set; } = string.Empty;

    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    // Navigation property
    public virtual ICollection<AttendanceRecord> AttendanceRecords { get; set; } =
        new List<AttendanceRecord>();
}
