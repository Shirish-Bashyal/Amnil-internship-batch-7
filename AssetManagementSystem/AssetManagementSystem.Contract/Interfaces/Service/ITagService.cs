using AssetManagementSystem.Shared.Dtos;
using AssetManagementSystem.Shared.Dtos.Tag;


namespace AssetManagementSystem.Contract.Interfaces.Service;

public interface  ITagService
{
    Task<ResponseData<IEnumerable<TagReadDto>>> GetAllAsync();
    Task<ResponseData<TagReadDto>> GetAsync(Guid id);
    Task<ResponseData<Guid>> CreateAsync(TagDto dto);
    Task<ResponseData<bool>> UpdateAsync(Guid id, TagDto dto);
    Task<ResponseData<bool>> DeleteAsync(Guid id);
}
