using AssetManagement.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagement.Contract.Service;

public interface IAssetService
{
    Task<ServiceResponseDto<AssetDto>> GetAsync(string id);
    Task<ServiceResponseDto<IEnumerable<AssetDto>>> GetAllAsync();
    Task<ServiceResponseDto<bool>> UpdateAsync(AssetDto assetDto);
    Task<ServiceResponseDto<bool>> DeleteAsync(AssetDto assetDto);
    Task<ServiceResponseDto<Guid>> CreateAsync(AssetDto assetDto);
    Task<ServiceResponseDto<IEnumerable<AssetDto>>>GetPagination(int  pageNumber, int pageSize);   
}
