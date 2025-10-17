using AssetManagementSystem.Shared.Dtos;

namespace AssetManagementSystem.Contracts.Assets;

/// <summary>
/// defines operations for asset entity
/// </summary>
public interface IAssetService
{
    Task<ResponseDto> CreateAsync(CreateAssetDto input);
    Task<ResponseDto> UpdateAsync(Guid id, UpdateAssetDto input);
    Task<ResponseDto> DeleteAsync(Guid id);

    Task<ResponseDto> GetAsync(Guid id);

    Task<ResponseDto> GetListAsync(PagedFilterRequestDto filter);

    Task<ResponseDto> GetAllAsync();

    Task<(Stream? Stream, string FileName)> ExportToExcelAsync();
}
