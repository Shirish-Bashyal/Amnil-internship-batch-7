using AssetManagement.Contract.Repository;
using AssetManagement.Contract.Service;
using AssetManagement.Domain.Dtos;
using AssetManagement.Domain.Dtos.AssetDto;
using AssetManagement.Domain.Entity.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AssetManagement.Application.Services;

public class AssetServices : IAssetService
{
    private readonly IGenericRepository<Asset> _assetRepo;
    private readonly ILogger<Asset> _logger;
    private readonly IGenericRepository<Category> _categoryRepo;
    public AssetServices(IGenericRepository<Asset> assetRepo,ILogger<Asset> logger, IGenericRepository<Category> categoryRepo)
    {
        _assetRepo = assetRepo;
        _logger = logger;
        _categoryRepo = categoryRepo;
    }
    public async Task<ServiceResponseDto<Guid>> CreateAsync(AssetDto assetDto)
    {
        if (assetDto == null)
        {
            _logger.LogWarning("Empty Assets from USer");
            return new ServiceResponseDto<Guid>
            {
                Success = false,
                Message = "Empty Asset"
            };
            //return new ServiceResponseDto<bool>
            //{
            //    IsSuccess = false,
            //    Message = "User not found"
            //};
        }
        try
        {
            var exist = await _assetRepo.GetQueryable().AnyAsync(x => x.SerialNumber == assetDto.SerialNumber);
            var existFk = await _categoryRepo.GetQueryable().AnyAsync(x => x.Id == assetDto.CategoryId);
            if (exist)
            {
                _logger.LogWarning("Existing asset");
                return new ServiceResponseDto<Guid>
                {
                    Success = false,
                    Message = "Existing Asset"
                };
            }
            if (!existFk)
            {
                _logger.LogWarning("NonExisting Category");
                return new ServiceResponseDto<Guid>
                {
                    Success = false,
                    Message = "NonExisting Category"
                };
            }
            var asset = new Asset
            {
                SerialNumber = assetDto.SerialNumber.Trim(),
                Id = Guid.NewGuid(),
                Name = assetDto.Name.Trim(),
                Cost = assetDto.Cost,
                CategoryId = assetDto.CategoryId
            };
            await _assetRepo.InsertAsync(asset);
            _logger.LogInformation("User Create successfully");
            return new ServiceResponseDto<Guid> { Message = "Asset Created successfully", Success = true, Data = asset.Id };


        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Occroed in Creating Asset ");
            throw;
        }
    }

    public async Task<ServiceResponseDto<IEnumerable<AssetDto>>> GetAllAsync()
    {
        try
        {
            var assetList= await _assetRepo.GetAllAsync();
            if (assetList == null)
            {
                _logger.LogInformation("No Asset Avilable");
                return new ServiceResponseDto<IEnumerable<AssetDto>> { Message = "No Asset", Success = true, Data = null };
            }
            var assets = assetList.Select(x => new AssetDto
            {
                SerialNumber = x.SerialNumber,
                Id = x.Id,
                Name = x.Name,
                Cost = x.Cost,
                CategoryId = x.CategoryId
            });
            _logger.LogInformation("Inforamtion Successfully Retrived");
            return new ServiceResponseDto<IEnumerable<AssetDto>> { Message = " Asset Retrived", Success = true, Data =assets };

        }catch(Exception ex)
        {
            _logger.LogInformation(ex, "Error occured");
            throw;
        }
        
    }

    public Task<ServiceResponseDto<bool>> DeleteAsync(AssetDto assetDto)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResponseDto<AssetDto>> GetAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResponseDto<IEnumerable<AssetDto>>> GetPagination(int pageNumber, int pageSize)
    {
        //var items = await query
        //.Skip((pageNumber - 1) * pageSize)
        //.Take(pageSize)
        //.Select(selector)
        //.ToListAsync();

        //return new PaginatedResult<TResult>
        //{
        //    Items = items,
        //    TotalCount = totalCount,
        //    PageNumber = pageNumber,
        //    PageSize = pageSize
        //};
        throw new NotImplementedException();
    }

    public Task<ServiceResponseDto<bool>> UpdateAsync(AssetDto assetDto)
    {
        throw new NotImplementedException();
    }
}
