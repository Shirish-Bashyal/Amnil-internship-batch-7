using AssetManagement.Contract.Repository;
using AssetManagement.Contract.Service;
using AssetManagement.Domain.Dtos;
using AssetManagement.Domain.Dtos.User;
using AssetManagement.Domain.Entity.Application;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Application.Services;

public class UserService : IUserServices
{
    private readonly ILogger _logger;   
    private readonly IGenericRepository<User> _userRepo;
    private readonly IGenericRepository<Department> _departmentRepo;
    public UserService(ILogger<User> logger,IGenericRepository<User> userRepo, IGenericRepository<Department> departmentRepo)
    {
        _logger = logger;
        _userRepo = userRepo;
        _departmentRepo = departmentRepo;
    }
    public async Task<ServiceResponseDto<Guid>> CreateAsync(CreateUserDto entity)
    {
        if(entity == null)
        {
            _logger.LogWarning("User Field is empty");
            return new ServiceResponseDto<Guid> { Message="User Field is empty",Success=false };
            
        }
        try
        {
            var user = new User
            {
                Id=Guid.NewGuid(),  
                Email = entity.Email,
                Name = entity.Name,
                DepartmentId = entity.DepartmentId,
            };
            var result = await _userRepo.InsertAsync(user);
            if (result!=null)
            {
                _logger.LogInformation("User Created");
                return new ServiceResponseDto<Guid> { Message = "User Created", Success = true,Data=user.Id };
            }
            _logger.LogWarning("User Field is empty");
            return new ServiceResponseDto<Guid> { Message = "User Field is empty", Success = false };
        }catch(Exception ex)
        {
            _logger.LogWarning("User Field is empty");
            return new ServiceResponseDto<Guid> { Message = "User Field is empty", Success = false };
        }

    }

    public async Task<ServiceResponseDto<bool>> DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            _logger.LogWarning("Is is empty");
            return new ServiceResponseDto<bool> { Message = "Id empty", Success = false };
        }
        try
        {
            var result = await _userRepo.DeleteAsync(id);
            if (result)
            {
                _logger.LogInformation("User Deleted");
                return new ServiceResponseDto<bool> { Message = "User deleted", Success = true };
            }
            _logger.LogWarning("User not deleted");
            return new ServiceResponseDto<bool> { Message = "User not deleted", Success = false };
        }
        catch(Exception ex)
        {
            _logger.LogWarning("User Field is empty");
            return new ServiceResponseDto<bool> { Message = "Some Error Occured", Success = false };
        }
    }

    public async Task<ServiceResponseDto<IEnumerable<GetUserDto>>> GetAllAsync()
    {
        try
        {
            var userList = await _userRepo.GetAllAsync();
            if (userList == null)
            {
                _logger.LogInformation("No user Avilable");
                return new ServiceResponseDto<IEnumerable<GetUserDto>> { Message = "No Asset", Success = true, Data = null };
            }
            var user = userList.Select(x => new GetUserDto
            {
                Name=x.Name,
                Email=x.Email,
                DepartmentId=x.DepartmentId
            });
            _logger.LogInformation("Inforamtion Successfully Retrived");
            return new ServiceResponseDto<IEnumerable<GetUserDto>> { Message = " Asset Retrived", Success = true, Data = user };

        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Error occured");
            throw;
        }
    }

    public Task<ServiceResponseDto<GetUserDto>> GetAsync(string id)
    {
        throw new NotImplementedException();
    }
}
