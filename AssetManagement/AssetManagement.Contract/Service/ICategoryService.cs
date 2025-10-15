using AssetManagement.Domain.Dtos;
using AssetManagement.Domain.Dtos.AssetDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Contract.Service;

public interface ICategoryService
{
    Task<ServiceResponseDto<CategoryDto>> GetAsync(string id);
    Task<ServiceResponseDto<IEnumerable<CategoryDto>>> GetAllAsync();
    Task<ServiceResponseDto<bool>> UpdateAsync(CategoryDto categoryDto);
    Task<ServiceResponseDto<bool>> DeleteAsync(CategoryDto categoryDto);
    Task<ServiceResponseDto<Guid>> CreateAsync(CategoryDto categoryDto);
    Task<ServiceResponseDto<IEnumerable<AssetDto>>> GetPagination(int pageNumber, int pageSize);
}
