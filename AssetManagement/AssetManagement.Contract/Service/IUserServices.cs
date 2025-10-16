using AssetManagement.Domain.Dtos;
using AssetManagement.Domain.Dtos.User;
using AssetManagement.Domain.Entity.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Contract.Service;

public interface IUserServices
{
    Task<ServiceResponseDto<GetUserDto>> GetAsync(string id);
    Task<ServiceResponseDto<IEnumerable<GetUserDto>>> GetAllAsync();
    Task<ServiceResponseDto<Guid>> CreateAsync(CreateUserDto entity);

    Task<ServiceResponseDto<bool>> DeleteAsync(Guid id);
}
