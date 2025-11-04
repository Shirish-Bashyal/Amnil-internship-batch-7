using AssetManagementSystem.Contracts.Assets;
using AssetManagementSystem.Shared.Dtos;

namespace AssetManagementSystem.Contracts.Tags;

/// <summary>
///defines operations for Tag entity
/// </summary>
public interface ITagService
{
    Task<ResponseDto> CreateAsync(CreateTagDto input);
    Task<ResponseDto> UpdateAsync(Guid id, UpdateTagDto input);
    Task<ResponseDto> DeleteAsync(Guid id);

    Task<ResponseDto> GetAsync(Guid id);

    Task<ResponseDto> GetAvailableListAsync();

    Task<ResponseDto> ChangeStatusAsync(Guid id, bool status);
    Task<ResponseDto> GetListAsync(PagedFilterRequestDto filter);

    Task<(Stream? Stream, string FileName)> ExportToExcelAsync();
}
