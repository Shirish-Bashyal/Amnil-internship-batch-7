using System.IO;
using AssetManagementSystem.Contracts.Assets;
using AssetManagementSystem.Contracts.Repositories;
using AssetManagementSystem.Domain.Entities.Assets;
using AssetManagementSystem.Domain.Entities.Categories;
using AssetManagementSystem.Domain.Entities.Departments;
using AssetManagementSystem.Domain.Entities.Tags;
using AssetManagementSystem.Shared.Constants;
using AssetManagementSystem.Shared.Dtos;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AssetManagementSystem.Application.Assets;

/// <summary>
/// Implementation of operations for asset entity
/// </summary>
public class AssetService : IAssetService
{
    private readonly IGenericRepository<Asset> _assetRepo;
    private readonly IGenericRepository<Category> _categoryRepo;
    private readonly IGenericRepository<Department> _departmentRepo;
    private readonly IGenericRepository<Tag> _tagRepo;

    private readonly ILogger<AssetService> _logger;

    public AssetService(
        IGenericRepository<Asset> assetRepo,
        ILogger<AssetService> logger,
        IGenericRepository<Category> categoryRepo,
        IGenericRepository<Department> departmentRepo,
        IGenericRepository<Tag> tagRepo
    )
    {
        _assetRepo = assetRepo;
        _logger = logger;
        _categoryRepo = categoryRepo;
        _departmentRepo = departmentRepo;
        _tagRepo = tagRepo;
    }

    /// <summary>
    /// Updates the existing asset entity
    /// </summary>
    public async Task<ResponseDto> UpdateAsync(Guid id, UpdateAssetDto input)
    {
        _logger.LogDebug("Updating Asset with Id {AssetId} ", id);

        input = input with
        {
            Name = input.Name.Trim(),
            SerialNumber = input.SerialNumber.Trim(),
            Description = input.Description?.Trim()
        };

        if (string.IsNullOrWhiteSpace(input.Name))
        {
            _logger.LogWarning("Asset Update failed due to invalid Name.");

            return ResponseDto.BadRequest("Asset Name is required.");
        }

        if (string.IsNullOrWhiteSpace(input.SerialNumber))
        {
            _logger.LogWarning("Asset update failed due to invalid Serial Number.");

            return ResponseDto.BadRequest("Asset Serial Number is required");
        }

        try
        {
            var normalizedName = input.Name.ToUpper();
            var normalizedSerialNumber = input.SerialNumber.ToUpper();

            var asset = await _assetRepo.GetAsync(id);
            if (asset == null)
            {
                _logger.LogWarning(" Asset with Id {AssetId} not found.", id);

                return ResponseDto.NotFound("Asset not found");
            }

            var duplicateName = await _assetRepo
                .GetQueryable()
                .AnyAsync(x => x.Id != id && x.NormalizedName == normalizedName);

            if (duplicateName)
            {
                _logger.LogWarning(
                    "Asset with Serial Number {SerialNumber} update failed. Asset Name  already exists.",
                    input.SerialNumber
                );
                return ResponseDto.BadRequest("Asset Name Already Exists");
            }

            var duplicateSerialNumber = await _assetRepo
                .GetQueryable()
                .AnyAsync(x => x.Id != id && x.NormalizedSerialNumber == normalizedSerialNumber);

            if (duplicateSerialNumber)
            {
                _logger.LogWarning(
                    "Asset with Serial Number {SerialNumber} update failed. Asset Serial Number already exists.",
                    input.SerialNumber
                );

                return ResponseDto.BadRequest("Asset Serial Number already exists");
            }

            var categoryExists = await _categoryRepo
                .GetQueryable()
                .AnyAsync(x => x.Id == input.CategoryId);

            if (!categoryExists)
            {
                _logger.LogWarning(
                    "Asset with Serial Number {SerialNumber} update  failed.{CategoryId} category not found.",
                    asset.SerialNumber,
                    input.CategoryId
                );

                return ResponseDto.NotFound("Category not found.");
            }

            if (input.DepartmentId.HasValue && input.DepartmentId != Guid.Empty)
            {
                var departmentExists = await _departmentRepo
                    .GetQueryable()
                    .AnyAsync(c => c.Id == input.DepartmentId);

                if (!departmentExists)
                {
                    _logger.LogWarning(
                        "Asset with Serial Number {SerialNumber} update failed. {DepartmentId} Department not found.",
                        input.SerialNumber,
                        input.DepartmentId
                    );

                    return ResponseDto.NotFound("Department not found.");
                }

                asset.DepartmentId = input.DepartmentId;
            }

            asset.Name = input.Name;
            asset.NormalizedName = normalizedName;
            asset.SerialNumber = input.SerialNumber;
            asset.NormalizedSerialNumber = normalizedSerialNumber;
            asset.CategoryId = input.CategoryId;
            asset.IsActive = input.IsActive;
            asset.Description = input.Description;
            asset.ReceivedDate = input.ReceivedDate;

            var isUpdated = await _assetRepo.UpdateAsync(asset);
            if (isUpdated)
            {
                _logger.LogInformation("Asset with Id {AssetId} updated successfully", id);

                return ResponseDto.Success("Asset updated successfully");
            }

            _logger.LogError("Unexpected error while updating asset");

            return ResponseDto.InternalServerError("An unexpected error occurred.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while updating asset.");

            return ResponseDto.InternalServerError("An unexpected error occurred.");
        }
    }

    /// <summary>
    /// creates Asset entity
    /// </summary>
    public async Task<ResponseDto> CreateAsync(CreateAssetDto input)
    {
        input = input with
        {
            Name = input.Name.Trim(),
            SerialNumber = input.SerialNumber.Trim(),
            Description = input.Description?.Trim(),
        };

        if (string.IsNullOrEmpty(input.Name))
        {
            _logger.LogWarning("Asset creation failed due to invalid Name.");

            return ResponseDto.BadRequest("Asset Name is required.");
        }

        if (string.IsNullOrEmpty(input.SerialNumber))
        {
            _logger.LogWarning("Asset creation failed due to invalid Serial Number.");

            return ResponseDto.BadRequest("Asset Serial Number is required.");
        }

        if (input.Image?.Length > AssetConsts.Image.FileSize)
        {
            _logger.LogWarning("File Size exceeds");

            return ResponseDto.BadRequest(
                $"File size exceeds limit of {AssetConsts.Image.FileSize / (1024 * 1024)} mb"
            );
        }

        if (input.Image != null && input.Image?.ContentType != AssetConsts.Image.PngFormat)
        {
            _logger.LogWarning("Invalid file format");

            return ResponseDto.BadRequest("Invalid Fileformat, only jpeg  format is accepted");
        }

        try
        {
            var normalizedName = input.Name.ToUpper();
            var normalizedSerialNumber = input.SerialNumber.ToUpper();

            var duplicateName = await _assetRepo
                .GetQueryable()
                .AnyAsync(x => x.NormalizedName == normalizedName);
            if (duplicateName)
            {
                _logger.LogWarning(
                    "Asset with Serial Number {SerialNumber} creation failed. Asset Name  already exists.",
                    input.SerialNumber
                );

                return ResponseDto.BadRequest("Asset Name already exists");
            }

            var duplicateSerialNumber = await _assetRepo
                .GetQueryable()
                .AnyAsync(x => x.NormalizedSerialNumber == normalizedSerialNumber);
            if (duplicateSerialNumber)
            {
                _logger.LogWarning(
                    "Asset with Serial Number {SerialNumber} creation failed. Asset Serial Number already exists.",
                    input.SerialNumber
                );

                return ResponseDto.BadRequest("Asset Serial Number already exists");
            }

            var hasCategory = await _categoryRepo
                .GetQueryable()
                .AnyAsync(x => x.Id == input.CategoryId);

            if (!hasCategory)
            {
                _logger.LogWarning(
                    "Asset with Serial Number {SerialNumber} creation failed.{CategoryId} category not found.",
                    input.SerialNumber,
                    input.CategoryId
                );

                return ResponseDto.NotFound("Category not found.");
            }

            var asset = new Asset();

            if (input.DepartmentId.HasValue && input.DepartmentId != Guid.Empty)
            {
                var departmentExists = await _departmentRepo
                    .GetQueryable()
                    .AnyAsync(c => c.Id == input.DepartmentId);

                if (!departmentExists)
                {
                    _logger.LogWarning(
                        "Asset with Serial Number {SerialNumber} creation failed. {DepartmentId} Department not found.",
                        input.SerialNumber,
                        input.DepartmentId
                    );

                    return ResponseDto.NotFound("Department not found.");
                }

                asset.DepartmentId = input.DepartmentId;
            }

            asset.Name = input.Name;
            asset.NormalizedName = normalizedName;
            asset.SerialNumber = input.SerialNumber;
            asset.NormalizedSerialNumber = normalizedSerialNumber;
            asset.CategoryId = input.CategoryId;
            asset.IsActive = true;
            asset.Description = input.Description;
            asset.ReceivedDate = input.ReceivedDate;

            if (input.Image != null && input.Image.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await input.Image.CopyToAsync(memoryStream);
                    asset.Image = memoryStream.ToArray();
                }
            }

            var result = await _assetRepo.InsertAsync(asset);

            var createdAsset = new AssetDto
            {
                Id = result.Id,
                Name = result.Name,
                SerialNumber = result.SerialNumber,
                IsActive = result.IsActive,
            };

            _logger.LogInformation(
                "Asset with Serial Number {SerialNumber} created successfully",
                input.SerialNumber
            );
            return ResponseDto<AssetDto>.Created(createdAsset, "Asset created successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while creating asset.");

            return ResponseDto.InternalServerError(
                "An unexpected error occurred while creating the asset."
            );
        }
    }

    /// <summary>
    /// Deletes a Asset based on Id.
    /// </summary>
    public async Task<ResponseDto> DeleteAsync(Guid id)
    {
        _logger.LogDebug("Deleting Asset with Id: {AssetId}", id);

        try
        {
            var asset = await _assetRepo.GetAsync(id);
            if (asset == null)
            {
                _logger.LogWarning(" Asset with Id {AssetId} not found for deletion", id);

                return ResponseDto.NotFound("Asset not found");
            }

            var isDeleted = await _assetRepo.DeleteAsync(asset);
            if (isDeleted)
            {
                _logger.LogInformation("Asset with Id {AssetId} deleted successfully", id);

                return ResponseDto.Success("Asset Successfully Deleted.");
            }

            _logger.LogWarning(
                " Asset with Id {AssetId} not deleted, Unexpected error while deleting asset.",
                id
            );

            return ResponseDto.InternalServerError(
                "An unexpected error occurred while creating the asset."
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while deleting asset.");

            return ResponseDto.InternalServerError(
                "An unexpected error occurred while creating the asset."
            );
        }
    }

    /// <summary>
    /// Retrives all Assets
    /// </summary>
    public async Task<ResponseDto> GetAllAsync()
    {
        _logger.LogDebug("Fetching all Asset");

        try
        {
            var assets = await _assetRepo.GetQueryable().ToDto().ToListAsync();

            _logger.LogInformation(" Assets retrieved successfully");

            return ResponseDto<IEnumerable<AssetDto>>.Success(
                assets,
                "Assets retrieved successfully"
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while fetching asset.");

            return ResponseDto.InternalServerError(
                "An unexpected error occurred while creating the asset."
            );
        }
    }

    /// <summary>
    ///  Retrive a Asset based on Id
    /// </summary>
    public async Task<ResponseDto> GetAsync(Guid id)
    {
        _logger.LogDebug("Fetching Asset by id: {AssetId}", id);

        try
        {
            var asset = await _assetRepo
                .GetQueryable()
                .Where(x => x.Id == id)
                .Select(x => new AssetDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    SerialNumber = x.SerialNumber,
                    DepartmentId = x.DepartmentId,
                    CategoryId = x.CategoryId,
                    Description = x.Description,
                    IsActive = x.IsActive,
                    ReceivedDate = x.ReceivedDate,
                    Category = x.Category.Name,
                    Department = x.Department.Name,
                    TagMacAddress = x.Tags.Select(x => x.MacAddress).ToList(),
                    ImageData = x.Image
                })
                .FirstOrDefaultAsync();

            if (asset == null)
            {
                _logger.LogWarning("Asset  with id {AssetId} not found", id);

                return ResponseDto.BadRequest("Asset Id not found");
            }

            _logger.LogInformation("Asset with id {AssetId} retrieved successfully", id);

            return ResponseDto<AssetDto>.Success(asset, "Asset Retrived Successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while fetching asset.");

            return ResponseDto.InternalServerError(
                "An unexpected error occurred while creating the asset."
            );
        }
    }

    /// <summary>
    /// Retrives user with pagination
    /// </summary>
    public async Task<ResponseDto> GetListAsync(PagedFilterRequestDto filter, AssetFilter input)
    {
        _logger.LogInformation(
            "Fetching {AssetCount} Asset with pagination",
            filter.MaxResultCount
        );
        try
        {
            var query = _assetRepo
                .GetQueryable()
                .Search(filter.SearchTerm)
                .Filter(input.CategoryId, filter.IsActive)
                .Sort(filter.SortOrder);

            var totalCount = await query.CountAsync();

            var assets = await query
                .ToDto()
                .Skip(filter.SkipCount)
                .Take(filter.MaxResultCount)
                .ToListAsync();

            var pagedResponse = new PagedResponseDto<AssetDto>
            {
                Items = assets,
                TotalCount = totalCount,
            };

            _logger.LogInformation(
                "{AssetCount} Assets retrieved successfully",
                filter.MaxResultCount
            );

            return ResponseDto<PagedResponseDto<AssetDto>>.Success(
                pagedResponse,
                "Assets retrieved successfully"
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while fetching asset.");

            return ResponseDto.InternalServerError(
                "An unexpected error occurred while creating the asset."
            );
        }
    }

    /// <summary>
    ///  retrives assets data in excel format
    /// </summary>
    public async Task<(Stream? Stream, string? FileName, string? ContentType)> ExportAsync(
        string format
    )
    {
        _logger.LogInformation("Exporting all Assets to Excel");

        try
        {
            var assets = await _assetRepo
                .GetQueryable()
                .Select(x => new AssetDto
                {
                    Name = x.Name,
                    SerialNumber = x.SerialNumber,
                    Description = x.Description,
                    IsActive = x.IsActive,
                    ReceivedDate = x.ReceivedDate,
                    Category = x.Category.Name,
                    Department = x.Department.Name,
                })
                .ToListAsync();

            if (assets.Count < 1)
            {
                _logger.LogWarning("No assets found to export.");
                return (null, string.Empty, string.Empty);
            }

            var exporter = AssetExporterFactory.GetExporter(format);

            var fileBytes = exporter.Export(assets);
            var stream = new MemoryStream(fileBytes);

            _logger.LogInformation(
                "Successfully created asset export file: {FileName}",
                exporter.FileName
            );
            return (stream, exporter.FileName, exporter.ContentType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred during asset export.");
            return (null, string.Empty, string.Empty);
        }
    }

    /// <summary>
    /// changes status of an asset as per given input
    /// </summary>
    public async Task<ResponseDto> ChangeStatusAsync(Guid id, bool isActive) //dto
    {
        _logger.LogInformation(
            "Changing Asset status to {IsActive} with Id {AssetId} ",
            isActive,
            id
        );

        try
        {
            var asset = await _assetRepo.GetAsync(id);

            if (asset == null)
            {
                _logger.LogInformation("Asset with Id {AssetId} not found", id);

                return ResponseDto.NotFound("Asset with Id {AssetId} not found");
            }

            asset.IsActive = isActive;

            var result = await _assetRepo.UpdateAsync(asset);

            if (result)
            {
                _logger.LogInformation(
                    "Asset with Id {AssetId} changed status to {IsActive}",
                    id,
                    isActive
                );
                return ResponseDto.Success("Asset  status changed");
            }

            _logger.LogError("An unexpected error occured while changing status of the asset");

            return ResponseDto.InternalServerError(
                "An unexpected error occurred while creating the asset."
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occured while changing status of the asset");

            return ResponseDto.InternalServerError(
                "An unexpected error occurred while creating the asset."
            );
        }
    }

    /// <summary>
    /// assigns a tag to an asset
    /// </summary>
    /// <param name="input"></param>
    public async Task<ResponseDto> AssignTagAsync(AssignTagDto input)
    {
        _logger.LogInformation(
            " Assigning Tag {TagId} to Asset {AssetId}",
            input.TagId,
            input.AssetId
        );
        try
        {
            var hasAsset = await _assetRepo.GetQueryable().AnyAsync(x => x.Id == input.AssetId);

            if (!hasAsset)
            {
                _logger.LogInformation("Asset with Id {AssetId} not found", input.AssetId);

                return ResponseDto.NotFound("Asset not found");
            }

            var tag = await _tagRepo.GetAsync(input.TagId);

            if (tag == null)
            {
                _logger.LogInformation("Tag {TagId} not found", input.TagId);

                return ResponseDto.NotFound("Tag not found");
            }

            if (tag.AssetId.HasValue)
            {
                _logger.LogInformation(
                    "Tag {TagId} is already assigned to another asset",
                    input.TagId
                );
                return ResponseDto.BadRequest("Tag is already assigned to another asset");
            }

            tag.AssetId = input.AssetId;

            var result = await _tagRepo.UpdateAsync(tag);
            if (result)
            {
                _logger.LogInformation(
                    "Asset {AssetId} is assigned with Tag {TagId}",
                    input.AssetId,
                    input.TagId
                );
                return ResponseDto.Success("Tag assigned sucessfully");
            }

            _logger.LogError("An unexpected error occured");
            return ResponseDto.InternalServerError("An Unexpected error occured");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occured");
            return ResponseDto.InternalServerError("An Unexpected error occured");
        }
    }

    /// <summary>
    /// removes a tag from an asset
    /// </summary>
    /// <param name="assetId"></param>
    /// <returns></returns>
    public async Task<ResponseDto> UnAssignTagAsync(Guid assetId)
    {
        _logger.LogInformation("Unassigning Tag for Asset {AssetId}", assetId);
        try
        {
            var hasAsset = await _assetRepo.GetQueryable().AnyAsync(x => x.Id == assetId);

            if (!hasAsset)
            {
                _logger.LogInformation("Asset with Id {AssetId} not found", assetId);

                return ResponseDto.NotFound("Asset not found");
            }

            var assignedTags = await _tagRepo
                .GetQueryable()
                .Where(x => x.AssetId == assetId)
                .ToListAsync();

            foreach (var tag in assignedTags)
            {
                tag.AssetId = null;
            }

            await _tagRepo.UpdateRangeAsync(assignedTags);

            _logger.LogInformation("Tags unassigned for Asset {AssetId}.", assetId);

            return ResponseDto.Success("Tags unassigned");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected Error occured");

            return ResponseDto.InternalServerError("An unexpected error occured");
        }
    }
}
