using AttendanceManagementSystem.Shared.Dtos.Attendance;

namespace AttendanceManagementSystem.Web.Models;

public class ReportViewModel
{
    public IEnumerable<AttendanceDto> Attendance { get; set; } = new List<AttendanceDto>();

    public string Name { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;
}
