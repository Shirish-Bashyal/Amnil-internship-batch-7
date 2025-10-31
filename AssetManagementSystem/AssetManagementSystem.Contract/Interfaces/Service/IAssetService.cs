using AssetManagementSystem.Shared;
using AssetManagementSystem.Shared.Dtos;
using AssetManagementSystem.Shared.Dtos.Asset;


namespace AssetManagementSystem.Contract.Interfaces.Service;

public interface IAssetService
{
    Task<ResponseData<IEnumerable<AssetReadDto>>> GetAllAsync();
    Task<ResponseData<AssetReadDto>> GetAsync(Guid id);
    Task<ResponseData<Guid>> CreateAsync(AssetDto dto);
    Task<ResponseData<bool>> UpdateAsync(Guid id, AssetDto dto);
    Task<ResponseData<bool>> DeleteAsync(Guid id);

    Task<ResponseData<PagedResponseDto<AssetReadDto>>> GetListAsync(PaginationAndFilterRequest filter);
    Task<ResponseData<bool>> ChangeActivationStatusAsync(Guid id, bool isActivated);

    Task<(Stream? Stream, string FileName)> ExportToExcelAsync();


    Task<ResponseData<List<AssetCountDto>>> GetDepartmentData();
}
