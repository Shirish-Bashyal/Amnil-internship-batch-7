using AssetManagement.Contract.Repository;
using AssetManagement.Contract.Service;
using AssetManagement.Domain.Dtos;
using AssetManagement.Domain.Dtos.AssetDto;
using AssetManagement.Domain.Entity.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AssetManagement.Application.Services;

public class AssetServices : IAssetService
{
    private readonly IGenericRepository<Asset> _assetRepo;
    private readonly ILogger<Asset> _logger;
    private readonly IGenericRepository<Category> _categoryRepo;
    private readonly IGenericRepository<Department> _departmentRepo;
    private readonly IGenericRepository<Tag> _tagRepo;
    private readonly IGenericRepository<User> _userRepo;
    public AssetServices(
        IGenericRepository<Asset> assetRepo, 
        ILogger<Asset> logger,
        IGenericRepository<Category> categoryRepo, 
        IGenericRepository<Department> departmentRepo, 
        IGenericRepository<Tag> tagRepo,
        IGenericRepository<User> userRepo
        )
    {
        _assetRepo = assetRepo;
        _logger = logger;
        _categoryRepo = categoryRepo;
        _departmentRepo = departmentRepo;
        _tagRepo = tagRepo;
        _userRepo = userRepo;
    }
    public async Task<ServiceResponseDto<Guid>> CreateAsync(CreateAssetDto assetDto)
    {
        if (assetDto==null)
        {
            _logger.LogWarning("CreateAssetDto is null. Asset creation aborted");
            return new ServiceResponseDto<Guid>
            {
                Success = false,
                Message = "Asset data is missing"
            };
        }
        if (string.IsNullOrWhiteSpace(assetDto.Name))
        {
            _logger.LogWarning("Asset name is empty");
            return new ServiceResponseDto<Guid>
            {
                Success = false,
                Message = "Asset name is required"
            };
        }
        try
        {
            var existSerialNumber = await _assetRepo.GetQueryable().AnyAsync(x => x.SerialNumber == assetDto.SerialNumber);
            if (existSerialNumber)
            {
                _logger.LogWarning("Duplicate serial number detected: {SerialNumber}", assetDto.SerialNumber);
                return new ServiceResponseDto<Guid>
                {
                    Success = false,
                    Message = "Asset with this serial number already exists."
                };
            }
            var category = await _categoryRepo.GetQueryable().FirstAsync(x => x.CategoryName == assetDto.CategoryName);
            var existForeignKeyCategory = await _categoryRepo.GetQueryable().AnyAsync(x => x.Id == category.Id);
            if (!existForeignKeyCategory)
            {
                _logger.LogWarning("Category not found: {CategoryName}", assetDto.CategoryName);
                return new ServiceResponseDto<Guid>
                {
                    Success = false,
                    Message = "Specified category does not exist."
                };
            }
            var tag = new Tag
            {
                TagId = Guid.NewGuid(),
                MacAddress = assetDto.MacAddress,
                IsActive = false
            };
            var duplicateMacAddress=await _tagRepo.GetQueryable().AnyAsync(x=>x.MacAddress==assetDto.MacAddress);
            if (duplicateMacAddress)
            {
                _logger.LogWarning("Duplicate MAC address detected: {MacAddress}", assetDto.MacAddress);
                return new ServiceResponseDto<Guid>
                {
                    Success = false,
                    Message = "Tag with this MAC address already exists."

                };
            }
            await _tagRepo.InsertAsync(tag);
           
            var asset = new Asset
            {
                SerialNumber = assetDto.SerialNumber.Trim(),
                Id = Guid.NewGuid(),
                Name = assetDto.Name.Trim(),
                IsActive = false,
                TagId = tag.TagId,
                Cost = assetDto.Cost,
                CategoryId =category.Id
            };
            await _assetRepo.InsertAsync(asset);
            _logger.LogInformation("User Create successfully");
            return new ServiceResponseDto<Guid> { Message = "Asset Created successfully", Success = true, Data = asset.Id };


        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the asset.");
            throw;
        }
    }

    public async Task<ServiceResponseDto<IEnumerable<AssetDto>>> GetAllAsync()
    {
        try
        {
            var assetList = await _assetRepo.GetAllAsync();
            if (assetList == null)
            {
                _logger.LogWarning("Asset not avilable");
                return new ServiceResponseDto<IEnumerable<AssetDto>> { Message = "No Asset in Database", Success = true, Data = null };
            }
            var assets = assetList.Select(x => new AssetDto
            {
                SerialNumber = x.SerialNumber,
                Id = x.Id,
                Name = x.Name,
                Cost = x.Cost,
                CategoryId = x.CategoryId
            });
            _logger.LogInformation("All Information about assets recived");
            return new ServiceResponseDto<IEnumerable<AssetDto>> { Message = " Asset List Retrived", Success = true, Data = assets };

        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "An unexpected error occurred during asset Fetching");
            throw;
        }

    }


    public Task<ServiceResponseDto<AssetDto>> GetAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponseDto<bool>> UpdateAsync(UpdateAssetDto assetDto)
    {
        try
        {

        if(assetDto == null)
        {
            _logger.LogWarning("Empty Asset");
            return new ServiceResponseDto<bool>
            {
                Success = false,
                Message = "Empty asset",
                Data=false
            };
        }
        var asset = await _assetRepo.GetAsync(assetDto.Id);
        if(asset == null)
        {
            _logger.LogWarning("NonExisting Asset");
            return new ServiceResponseDto<bool>
            {
                Success = false,
                Message = "NonExisting asset",
                Data=false
            };
        }
        asset.UserId = assetDto.UserId;
        asset.Name = assetDto.Name;
        asset.IsActive = assetDto.Status;
        asset.Cost = assetDto.Cost;
        var result= await _assetRepo.UpdateAsync(asset);
        if (result)
        {
            _logger.LogWarning("Asset updated");
            return new ServiceResponseDto<bool>
            {
                Success = true,
                Message = "asset updated",
                Data=true
            };
        }
        _logger.LogWarning("could no change asset");
        return new ServiceResponseDto<bool>
        {
            Success = false,
            Message = "NonExisting asset false result"
        };
        }
        catch (Exception)
        {
            _logger.LogWarning("exception");
            return new ServiceResponseDto<bool>
            {
                Success = false,
                Message = "exception"
            };
        }

    }
    public async Task<ServiceResponseDto> DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            _logger.LogWarning("Input Guid From user is Empty");
            return new ServiceResponseDto
            {
                Success = false,
                Message = "Asset id Required"
            };
        }
        try
        {
            var isDeleted = await _assetRepo.DeleteAsync(id);
            if (!isDeleted)
            {
                _logger.LogWarning("The asset with {AssetId} is not found",id);
                return new ServiceResponseDto
                {
                    Success = false,
                    Message = "No Asset with such Id is Avilable"
                };
            }
            _logger.LogInformation("Asset with {AssetId}Deleted successfully",id);
            return new ServiceResponseDto
            {
                Success = true,
                Message = "Asset deleted"
            };
        }catch(Exception ex)
        {
            _logger.LogWarning(ex,"Unexpected Error Occured");
            return new ServiceResponseDto
            {
                Success = false,
                Message = "Error occured"
            };
        }
    }

    public Task<ServiceResponseDto<AssetDto>> GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }


    public Task<ServiceResponseDto> BulkInsert(IEnumerable<AssetDto> assetDto)
    {
        throw new NotImplementedException();
    }

    public async Task<PageResponse<GetAssetDto>> GetFilteredContent(PageRequest paged)
    {
        try
        {

        var query = await _assetRepo.GetAllAsync();

        var pagedAssets = query
            .Skip((paged.SkipPageCount - 1) * paged.ListCount)
            .Take(paged.ListCount)
            .ToList();

        var assetDtos = await Task.WhenAll(pagedAssets.Select(async x =>
        {
            var department = await _departmentRepo.GetQueryable().FirstOrDefaultAsync(u => u.Id == x.DepartmentId);
            var category = await _categoryRepo.GetQueryable().FirstOrDefaultAsync(u => u.Id == x.CategoryId);
            var user = await _userRepo.GetQueryable().FirstOrDefaultAsync(u => u.Id == x.UserId);

            return new GetAssetDto
            {
                Name = x.Name,
                CategoryId = x.CategoryId,
                SerialNumber = x.SerialNumber,
                Cost = x.Cost,
                Status = x.IsActive,
                DepartmentName = department?.Name ?? "Not Set",
                CategoryName = category?.CategoryName ?? "",
                UserName = user?.Name ?? "No User"
            };
        }));

        return new PageResponse<GetAssetDto>
        {
            Items = assetDtos.ToList(),
            TotalCount = query.Count
        };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the asset.");
            throw;

        }
    }

}
