using AttendanceManagementSystem.Contracts.Interfaces.Attendance;
using AttendanceManagementSystem.Contracts.Repository;
using AttendanceManagementSystem.Domain.Entities.Application;
using AttendanceManagementSystem.Shared.Constants.Enums;
using AttendanceManagementSystem.Shared.Dtos;
using AttendanceManagementSystem.Shared.Dtos.Attendance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AttendanceManagementSystem.Application.Services;

/// <summary>
/// Provides bussiness logic for managing user attendance
/// </summary>
public class AttendanceService : IAttendanceService
{
    private readonly IGenericRepository<AttendanceRecord> _attendanceRepo;
    private readonly ILogger<AttendanceRecord> _logger;
    private readonly IGenericRepository<User> _userRepo;

    public AttendanceService(
        IGenericRepository<AttendanceRecord> attendanceRepo,
        ILogger<AttendanceRecord> logger,
        IGenericRepository<User> userRepo
    )
    {
        _attendanceRepo = attendanceRepo;
        _logger = logger;
        _userRepo = userRepo;
    }

    public async Task<ServiceResponseDto<AttendanceDto>> CheckInAsync(Guid userId)
    {
        User? user = await _userRepo.GetAsync(userId);
        if (user == null)
        {
            return new ServiceResponseDto<AttendanceDto>
            {
                IsSuccess = false,
                Message = "User not found",
            };
        }
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var existing = await _attendanceRepo
            .GetQueryable()
            .Where(x => x.UserId == userId && x.Date == today)
            .FirstOrDefaultAsync();
        if (existing != null)
        {
            return new ServiceResponseDto<AttendanceDto>
            {
                IsSuccess = false,
                Message = "User is already CheckedIn"
            };
        }
        var record = new AttendanceRecord
        {
            UserId = user.Id,
            Date = today,
            CheckInTime = TimeOnly.FromDateTime(DateTime.UtcNow),
            Status = (int)AttendanceStatus.Present,
        };

        var result = await _attendanceRepo.InsertAsync(record);

        return new ServiceResponseDto<AttendanceDto>
        {
            IsSuccess = true,
            Data = new AttendanceDto
            {
                Date = result.Date,
                CheckInTime = result.CheckInTime,
                CheckOutTime = result.CheckOutTime,
                Status = ((AttendanceStatus)result.Status).ToString(),
            }
        };
    }

    public async Task<ServiceResponseDto<AttendanceDto>> CheckOutAsync(Guid userId)
    {
        var user = await _userRepo.GetAsync(userId);
        if (user == null)
        {
            return new ServiceResponseDto<AttendanceDto>
            {
                IsSuccess = false,
                Message = "User not found",
            };
        }
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var existingRecord = await _attendanceRepo
            .GetQueryable()
            .Where(x => x.UserId == userId && x.Date == today)
            .FirstOrDefaultAsync();

        if (existingRecord == null)
        {
            return new ServiceResponseDto<AttendanceDto>
            {
                IsSuccess = false,
                Message = "User is not CheckedIn"
            };
        }
        if (existingRecord.CheckOutTime != null)
        {
            return new ServiceResponseDto<AttendanceDto>
            {
                IsSuccess = false,
                Message = "User is already checkedOut"
            };
        }
        existingRecord.CheckOutTime = TimeOnly.FromDateTime(DateTime.UtcNow);

        var result = await _attendanceRepo.UpdateAsync(existingRecord);
        if (result)
        {
            return new ServiceResponseDto<AttendanceDto>
            {
                IsSuccess = result,
                Data = new AttendanceDto
                {
                    Date = existingRecord.Date,
                    CheckInTime = existingRecord.CheckInTime,
                    CheckOutTime = existingRecord.CheckOutTime,
                    Status = ((AttendanceStatus)existingRecord.Status).ToString()
                }
            };
        }
        return new ServiceResponseDto<AttendanceDto>
        {
            IsSuccess = result,
            Message = "Error occured, Try again"
        };
    }

    public async Task<ServiceResponseDto<IEnumerable<AttendanceDto>>> GetByUserIdAsync(Guid userId)
    {
        var records = await _attendanceRepo
            .GetQueryable()
            .Where(x => x.UserId == userId)
            .Select(x => new AttendanceDto
            {
                Date = x.Date,
                CheckInTime = x.CheckInTime,
                CheckOutTime = x.CheckOutTime,
                Status = ((AttendanceStatus)x.Status).ToString()
            })
            .ToListAsync();
        return new ServiceResponseDto<IEnumerable<AttendanceDto>>
        {
            IsSuccess = true,
            Data = records
        };
    }
}
