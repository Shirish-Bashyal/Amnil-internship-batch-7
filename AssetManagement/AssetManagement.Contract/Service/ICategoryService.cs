using AssetManagement.Domain.Dtos;
using AssetManagement.Domain.Dtos.AssetDto;

namespace AssetManagement.Contract.Service;

public interface ICategoryService
{
    Task<ServiceResponseDto<CategoryDto>> GetAsync(string id);
    Task<ServiceResponseDto<IEnumerable<CategoryDto>>> GetAllAsync();
    Task<ServiceResponseDto<bool>> UpdateAsync(CategoryDto categoryDto);
    Task<ServiceResponseDto<bool>> DeleteAsync(CategoryDto categoryDto);
    Task<ServiceResponseDto<Guid>> CreateAsync(CategoryDto categoryDto);
}
