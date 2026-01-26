using AttendanceManagementSystem.Shared.Dtos.User;

namespace AttendanceManagementSystem.Web.Models;

public class UserViewModel
{
    public int TotalItems { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }

    public List<GetUserDto> User { get; set; } = new List<GetUserDto>();
}
