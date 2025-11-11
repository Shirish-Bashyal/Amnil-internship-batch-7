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

    Task<ResponseDto> GetListAsync(PagedFilterRequestDto filter, AssetFilter input);

    Task<ResponseDto> GetAllAsync();

    Task<ResponseDto> ChangeStatusAsync(Guid id, bool isActive);

    Task<ResponseDto> AssignTagAsync(AssignTagDto input);

    Task<ResponseDto> UnAssignTagAsync(Guid assetId);

    Task<(Stream? Stream, string? FileName, string? ContentType)> ExportAsync(string format);
}
