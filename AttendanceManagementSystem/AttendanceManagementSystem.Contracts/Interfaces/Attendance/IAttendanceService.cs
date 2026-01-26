using AttendanceManagementSystem.Shared.Dtos;
using AttendanceManagementSystem.Shared.Dtos.Attendance;

namespace AttendanceManagementSystem.Contracts.Interfaces.Attendance;

public interface IAttendanceService
{
    Task<ServiceResponseDto<AttendanceDto>> CheckInAsync(Guid userId);

    Task<ServiceResponseDto<AttendanceDto>> CheckOutAsync(Guid userId);

    Task<ServiceResponseDto<IEnumerable<AttendanceDto>>> GetByUserIdAsync(Guid userId); //
}
