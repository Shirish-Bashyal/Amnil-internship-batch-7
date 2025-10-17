using System.IO;
using AssetManagementSystem.Contracts.Assets;
using AssetManagementSystem.Contracts.Repositories;
using AssetManagementSystem.Domain.Entities.Assets;
using AssetManagementSystem.Domain.Entities.Categories;
using AssetManagementSystem.Domain.Entities.Departments;
using AssetManagementSystem.Shared.Constants.Enums;
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

    private readonly ILogger<AssetService> _logger;

    public AssetService(
        IGenericRepository<Asset> assetRepo,
        ILogger<AssetService> logger,
        IGenericRepository<Category> categoryRepo,
        IGenericRepository<Department> departmentRepo
    )
    {
        _assetRepo = assetRepo;
        _logger = logger;
        _categoryRepo = categoryRepo;
        _departmentRepo = departmentRepo;
    }

    /// <summary>
    /// Updates the existing asset entity
    /// </summary>
    public async Task<ResponseDto> UpdateAsync(Guid id, UpdateAssetDto input)
    {
        _logger.LogDebug("Updating Asset with Id {AssetId} ", id);

        input.Name = input.Name.Trim();
        input.SerialNumber = input.SerialNumber.Trim();
        input.Description = input.Description?.Trim();

        if (string.IsNullOrEmpty(input.Name))
        {
            _logger.LogWarning("Asset Update failed due to invalid Name.");
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Asset Name is required.",
                StatusCode = (int)StatusCode.BadRequest,
            };
        }

        if (string.IsNullOrEmpty(input.SerialNumber))
        {
            _logger.LogWarning("Asset update failed due to invalid Serial Number.");
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Asset Serial Number is required",
                StatusCode = (int)StatusCode.BadRequest,
            };
        }

        try
        {
            var asset = await _assetRepo.GetAsync(id);
            if (asset == null)
            {
                _logger.LogWarning(" Asset with Id {AssetId} not found.", id);
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Asset not found",
                    StatusCode = (int)StatusCode.NotFound,
                };
            }

            var duplicateName = await _assetRepo
                .GetQueryable()
                .AnyAsync(x => x.Id != id && x.NormalizedName == input.Name.ToUpper());

            if (duplicateName)
            {
                _logger.LogWarning(
                    "Asset with Serial Number {SerialNumber} update failed. Asset Name  already exists.",
                    asset.SerialNumber
                );

                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Asset Name already exists",
                    StatusCode = (int)StatusCode.BadRequest,
                };
            }

            var duplicateSerialNumber = await _assetRepo
                .GetQueryable()
                .AnyAsync(x =>
                    x.Id != id && x.NormalizedSerialNumber == input.SerialNumber.ToUpper()
                );

            if (duplicateSerialNumber)
            {
                _logger.LogWarning(
                    "Asset with Serial Number {SerialNumber} update failed. Asset Serial Number already exists.",
                    asset.SerialNumber
                );

                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Asset Serial Number already exists",
                    StatusCode = (int)StatusCode.BadRequest,
                };
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
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Category not found.",
                    StatusCode = (int)StatusCode.NotFound
                };
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
                        asset.SerialNumber,
                        input.DepartmentId
                    );
                    return new ResponseDto
                    {
                        IsSuccess = false,
                        Message = "Department not found.",
                        StatusCode = (int)StatusCode.NotFound
                    };
                }

                asset.DepartmentId = input.DepartmentId;
            }

            asset.Name = input.Name;
            asset.NormalizedName = input.Name.ToUpper();
            asset.SerialNumber = input.SerialNumber;
            asset.NormalizedSerialNumber = input.SerialNumber.ToUpper();
            asset.CategoryId = input.CategoryId;
            asset.IsActive = true;
            asset.Description = input.Description;

            var isUpdated = await _assetRepo.UpdateAsync(asset);
            if (isUpdated)
            {
                _logger.LogInformation("Asset with Id {AssetId} updated successfully", id);
                return new ResponseDto
                {
                    IsSuccess = true,
                    Message = "Asset updated successfully",
                    StatusCode = (int)StatusCode.Success
                };
            }

            _logger.LogError("Unexpected error while updating asset");
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "An unexpected error occurred.",
                StatusCode = (int)StatusCode.InternalServerError
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while updating asset.");

            return new ResponseDto
            {
                IsSuccess = false,
                Message = "An unexpected error occurred .",
                StatusCode = (int)StatusCode.InternalServerError
            };
        }
    }

    /// <summary>
    /// creates Asset entity
    /// </summary>
    public async Task<ResponseDto> CreateAsync(CreateAssetDto input)
    {
        input.Name = input.Name.Trim();
        input.SerialNumber = input.SerialNumber.Trim();
        input.Description = input.Description?.Trim();

        if (string.IsNullOrEmpty(input.Name))
        {
            _logger.LogWarning("Asset creation failed due to invalid Name.");
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Asset Name is required.",
                StatusCode = (int)StatusCode.BadRequest,
            };
        }
        if (string.IsNullOrEmpty(input.SerialNumber))
        {
            _logger.LogWarning("Asset creation failed due to invalid Serial Number.");
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Asset Serial Number is required",
                StatusCode = (int)StatusCode.BadRequest,
            };
        }

        try
        {
            var duplicateName = await _assetRepo
                .GetQueryable()
                .AnyAsync(x => x.NormalizedName == input.Name.ToUpper());
            if (duplicateName)
            {
                _logger.LogWarning(
                    "Asset with Serial Number {SerialNumber} creation failed. Asset Name  already exists.",
                    input.SerialNumber
                );

                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Asset Name already exists",
                    StatusCode = (int)StatusCode.BadRequest,
                };
            }

            var duplicateSerialNumber = await _assetRepo
                .GetQueryable()
                .AnyAsync(x => x.NormalizedSerialNumber == input.SerialNumber.ToUpper());
            if (duplicateSerialNumber)
            {
                _logger.LogWarning(
                    "Asset with Serial Number {SerialNumber} creation failed. Asset Serial Number already exists.",
                    input.SerialNumber
                );

                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Asset Serial Number already exists",
                    StatusCode = (int)StatusCode.BadRequest,
                };
            }

            var categoryExists = await _categoryRepo
                .GetQueryable()
                .AnyAsync(x => x.Id == input.CategoryId);

            if (!categoryExists)
            {
                _logger.LogWarning(
                    "Asset with Serial Number {SerialNumber} creation failed.{CategoryId} category not found.",
                    input.SerialNumber,
                    input.CategoryId
                );
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Category not found.",
                    StatusCode = (int)StatusCode.NotFound
                };
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
                    return new ResponseDto
                    {
                        IsSuccess = false,
                        Message = "Department not found.",
                        StatusCode = (int)StatusCode.NotFound
                    };
                }

                asset.DepartmentId = input.DepartmentId;
            }

            asset.Name = input.Name;
            asset.NormalizedName = input.Name.ToUpper();
            asset.SerialNumber = input.SerialNumber;
            asset.NormalizedSerialNumber = input.SerialNumber.ToUpper();
            asset.CategoryId = input.CategoryId;
            asset.IsActive = true;
            asset.Description = input.Description;

            if (input.ReceivedDate.HasValue)
            {
                asset.ReceivedDate = input.ReceivedDate.Value;
            }

            var createdAsset = await _assetRepo.InsertAsync(asset);

            _logger.LogInformation(
                "Asset with Serial Number {SerialNumber} created successfully",
                input.SerialNumber
            );
            return new ResponseDto<AssetDto>
            {
                IsSuccess = true,
                Message = "Asset created successfully.",
                StatusCode = (int)StatusCode.Created,
                Data = new AssetDto
                {
                    Id = createdAsset.Id,
                    Name = createdAsset.Name,
                    SerialNumber = createdAsset.SerialNumber,
                    IsActive = createdAsset.IsActive,
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while creating asset.");
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "An unexpected error occurred while creating the asset.",
                StatusCode = (int)StatusCode.InternalServerError
            };
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
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Asset not found",
                    StatusCode = (int)StatusCode.BadRequest
                };
            }

            var isDeleted = await _assetRepo.DeleteAsync(asset);
            if (isDeleted)
            {
                _logger.LogInformation("Asset with Id {AssetId} deleted successfully", id);
                return new ResponseDto
                {
                    IsSuccess = true,
                    Message = "Asset Successfully Deleted.",
                    StatusCode = (int)StatusCode.Success
                };
            }

            _logger.LogWarning(
                " Asset with Id {AssetId} not deletd, Unexpected error while ddeleting asset.",
                id
            );

            return new ResponseDto
            {
                IsSuccess = false,
                Message = "An unexpected error occurred while deleting the asset.",
                StatusCode = (int)StatusCode.InternalServerError
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while deleting asset.");

            return new ResponseDto
            {
                IsSuccess = false,
                Message = "An unexpected error occurred while deleting the asset.",
                StatusCode = (int)StatusCode.InternalServerError
            };
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
            var assets = await _assetRepo
                .GetQueryable()
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
                    Department = x.Department.Name
                })
                .ToListAsync();

            _logger.LogInformation(" Assets retrieved successfully");

            return new ResponseDto<IEnumerable<AssetDto>>
            {
                IsSuccess = true,
                Message = "Assets retrieved successfully",
                Data = assets,
                StatusCode = (int)StatusCode.Success
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while fetching asset.");

            return new ResponseDto
            {
                IsSuccess = false,
                Message = "An unexpected error occurred .",
                StatusCode = (int)StatusCode.InternalServerError
            };
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
                    Department = x.Department.Name
                })
                .FirstOrDefaultAsync();

            if (asset == null)
            {
                _logger.LogWarning("Asset  with id {AssetId} not found", id);
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Asset Id not found",
                    StatusCode = (int)StatusCode.BadRequest
                };
            }

            _logger.LogInformation("Asset with id {AssetId} retrieved successfully", id);

            return new ResponseDto<AssetDto>
            {
                IsSuccess = true,
                Message = "Asset Retrived Successfully",
                Data = asset,
                StatusCode = (int)StatusCode.Success
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while fetching asset.");

            return new ResponseDto
            {
                IsSuccess = false,
                Message = "An unexpected error occurred .",
                StatusCode = (int)StatusCode.InternalServerError
            };
        }
    }

    /// <summary>
    /// Retrives user with pagination
    /// </summary>
    public async Task<ResponseDto> GetListAsync(PagedFilterRequestDto filter)
    {
        _logger.LogInformation(
            "Fetching {AssetCount} Asset with pagination",
            filter.MaxResultCount
        );
        try
        {
            var query = _assetRepo.GetQueryable();

            filter.SearchTerm = filter.SearchTerm?.Trim().ToUpper();

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                query = query.Where(x =>
                    x.NormalizedName.Contains(filter.SearchTerm)
                    || x.NormalizedSerialNumber.Contains(filter.SearchTerm)
                );
            }
            if (!string.IsNullOrWhiteSpace(filter.SortOrder))
            {
                query = filter.SortOrder switch
                {
                    "name_desc" => query.OrderByDescending(u => u.Name),
                    "name_asc" => query.OrderBy(x => x.Name),

                    "date_asc" => query.OrderBy(u => u.ReceivedDate),
                    "date_desc" => query.OrderByDescending(u => u.ReceivedDate),
                    "serialNumber_desc" => query.OrderByDescending(x => x.SerialNumber),
                    "serialNumber_asc" => query.OrderBy(x => x.SerialNumber),
                    _ => query.OrderByDescending(u => u.ReceivedDate)
                };
            }
            var totalCount = await query.CountAsync();

            var assets = await query
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
                    Department = x.Department.Name
                })
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

            return new ResponseDto<PagedResponseDto<AssetDto>>
            {
                Data = pagedResponse,
                IsSuccess = true,
                Message = "Assets retrieved successfully",
                StatusCode = (int)StatusCode.Success,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while fetching asset.");

            return new ResponseDto
            {
                IsSuccess = false,
                Message = "An unexpected error occurred .",
                StatusCode = (int)StatusCode.InternalServerError
            };
        }
    }

    /// <summary>
    ///  retrives assets data in excel format
    /// </summary>

    public async Task<(Stream? Stream, string FileName)> ExportToExcelAsync()
    {
        _logger.LogInformation("Exporting all Assets to Excel");

        try
        {
            var assets = await _assetRepo
                .GetQueryable()
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
                    Department = x.Department.Name
                })
                .ToListAsync();

            if (assets.Count < 1)
            {
                _logger.LogWarning("No assets found to export.");
                return (null, string.Empty);
            }

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Assets");

            worksheet.Cell(1, 1).Value = "Asset ID";
            worksheet.Cell(1, 2).Value = "Asset Name";
            worksheet.Cell(1, 3).Value = "Serial Number";
            worksheet.Cell(1, 4).Value = "Description";
            worksheet.Cell(1, 5).Value = "Is Active";
            worksheet.Cell(1, 6).Value = "Received Date";
            worksheet.Cell(1, 7).Value = "Category ID";
            worksheet.Cell(1, 8).Value = "Category Name";
            worksheet.Cell(1, 9).Value = "Department ID";
            worksheet.Cell(1, 10).Value = "Department Name";

            var dataToInsert = assets.Select(a =>
                new object[]
                {
                    a.Id,
                    a.Name,
                    a.SerialNumber,
                    a.Description ?? string.Empty,
                    a.IsActive,
                    a.ReceivedDate,
                    a.CategoryId,
                    a.Category ?? string.Empty,
                    a.DepartmentId,
                    a.Department ?? string.Empty
                }
            );

            worksheet.Cell(2, 1).InsertData(dataToInsert);
            worksheet.Columns().AdjustToContents();
            worksheet.Column(6).Style.DateFormat.SetFormat("yyyy-MM-dd");

            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;
            var fileName = $"Assets_Export_{DateTime.UtcNow:yyyyMMdd_HHmmss}.xlsx";
            _logger.LogInformation("Successfully created asset export file: {FileName}", fileName);
            return (stream, fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred during asset export.");
            return (null, string.Empty);
        }
    }
}
