using AttendanceManagementSystem.Shared.Dtos;
using AttendanceManagementSystem.Shared.Dtos.User;

namespace AttendanceManagementSystem.Contracts.Interfaces.User;

public interface IUserService
{
    Task<ServiceResponseDto<Guid>> CreateAsync(UserDto model);

    Task<ServiceResponseDto<bool>> UpdateAsync(Guid id, UserDto model);

    Task<ServiceResponseDto<bool>> DeleteAsync(Guid id);

    Task<ServiceResponseDto<IEnumerable<GetUserDto>>> GetAllAsync();

    Task<ServiceResponseDto<PagedResponseDto<GetUserDto>>> GetListAsync(
        PagedRequestDto input,
        string? searchTerm,
        string? sortOrder
    );

    Task<ServiceResponseDto<GetUserDto>> GetByIdAsync(Guid id);
}
