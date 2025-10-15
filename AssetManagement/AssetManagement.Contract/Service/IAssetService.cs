using AssetManagement.Domain.Dtos;
using AssetManagement.Domain.Dtos.AssetDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Contract.Service;

public interface IAssetService
{
    Task<ServiceResponseDto<AssetDto>> GetAsync(Guid id);
    Task<ServiceResponseDto<bool>> UpdateAsync(AssetDto assetDto);
    Task<ServiceResponseDto<bool>> DeleteAsync(Guid id);
    Task<ServiceResponseDto<Guid>> CreateAsync(AssetDto assetDto);
    Task <PageResponse<AssetDto>> GetPagination(PageRequest paged);   
}
