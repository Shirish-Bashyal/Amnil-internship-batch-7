using AssetManagement.Domain.Dtos;
using AssetManagement.Domain.Dtos.AssetDto;

namespace AssetManagement.Contract.Service;

public interface IAssetService
{
    Task<ServiceResponseDto<IEnumerable<AssetDto>>> GetAllAsync();
    Task<ServiceResponseDto>BulkInsert(IEnumerable<AssetDto> assetDto);
    Task<ServiceResponseDto<AssetDto>> GetAsync(Guid id);
    Task<ServiceResponseDto<bool>> UpdateAsync(UpdateAssetDto assetDto);
    Task<ServiceResponseDto> DeleteAsync(Guid id);
    Task<ServiceResponseDto<Guid>> CreateAsync(CreateAssetDto assetDto);
    Task<PageResponse<GetAssetDto>> GetFilteredContent(PageRequest paged);
}
