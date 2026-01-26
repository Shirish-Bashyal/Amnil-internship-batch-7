using System.ComponentModel.DataAnnotations.Schema;
using AttendanceManagementSystem.Domain.Entities.Base;

namespace AttendanceManagementSystem.Domain.Entities.Application;

/// <summary>
/// Represents a single attendance entry for a user.
/// </summary>
public class AttendanceRecord : Entity<Guid>
{
    public Guid UserId { get; set; }

    public DateOnly Date { get; set; }
    public TimeOnly? CheckInTime { get; set; }
    public TimeOnly? CheckOutTime { get; set; }

    public int Status { get; set; } // Present= 1, Absent =0, HalfDay

    // Navigation property
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
}
