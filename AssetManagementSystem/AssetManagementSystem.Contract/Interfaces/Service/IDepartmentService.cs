using AssetManagementSystem.Shared.Dtos;
using AssetManagementSystem.Shared.Dtos.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagementSystem.Contract.Interfaces.Service;

public interface IDepartmentService
{
    Task<ResponseData<IEnumerable<DepartmentReadDto>>> GetAllAsync();
    Task<ResponseData<DepartmentReadDto>> GetAsync(Guid id);
    Task<ResponseData<Guid>> CreateAsync(DepartmentDto dto);
    Task<ResponseData<bool>> UpdateAsync(Guid id, DepartmentDto dto);
    Task<ResponseData<bool>> DeleteAsync(Guid id);
}
