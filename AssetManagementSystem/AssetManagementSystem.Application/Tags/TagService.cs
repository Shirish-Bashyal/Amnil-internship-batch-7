using AssetManagementSystem.Contracts.Assets;
using AssetManagementSystem.Contracts.Repositories;
using AssetManagementSystem.Contracts.Tags;
using AssetManagementSystem.Domain.Entities.Assets;
using AssetManagementSystem.Domain.Entities.Tags;
using AssetManagementSystem.Shared.Constants.Enums;
using AssetManagementSystem.Shared.Dtos;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AssetManagementSystem.Application.Tags;

/// <summary>
/// Implementations of operations for Tag entity.
/// </summary>
public class TagService : ITagService
{
    private readonly IGenericRepository<Tag> _tagRepo;
    private readonly ILogger<TagService> _logger;

    public TagService(IGenericRepository<Tag> tagRepo, ILogger<TagService> logger)
    {
        _tagRepo = tagRepo;
        _logger = logger;
    }

    /// <summary>
    /// creates Tag entity
    /// </summary>

    public async Task<ResponseDto> CreateAsync(CreateTagDto input)
    {
        _logger.LogInformation(
            "Tag with Mac address {MacAddress} creation begin",
            input.MacAddress
        );

        input.MacAddress = input.MacAddress.Trim().ToUpper();
        if (string.IsNullOrEmpty(input.MacAddress))
        {
            _logger.LogWarning("Tag Creation failed due to invalid Mac address");
            return new ResponseDto()
            {
                IsSuccess = false,
                Message = "Mac Address is required",
                StatusCode = (int)StatusCode.BadRequest,
            };
        }

        try
        {
            var duplicateMac = await _tagRepo
                .GetQueryable()
                .AnyAsync(x => x.MacAddress == input.MacAddress);
            if (duplicateMac)
            {
                _logger.LogWarning("Mac Address {MacAddress} already exists", input.MacAddress);
                return new ResponseDto()
                {
                    IsSuccess = false,
                    Message = "Mac Address is already exists",
                    StatusCode = (int)StatusCode.BadRequest,
                };
            }

            var tag = new Tag { MacAddress = input.MacAddress, IsActive = true };

            var createdMac = await _tagRepo.InsertAsync(tag);

            _logger.LogInformation(
                "Tag with Mac Address {MacAddress} created successfully",
                input.MacAddress
            );
            return new ResponseDto<TagDto>
            {
                IsSuccess = true,
                Message = "Tag created successfully.",
                StatusCode = (int)StatusCode.Created,
                Data = new TagDto
                {
                    Id = createdMac.Id,
                    IsActive = createdMac.IsActive,
                    MacAddress = createdMac.MacAddress,
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while creating Tag.");

            return new ResponseDto
            {
                IsSuccess = false,
                Message = "An unexpected error occurred.",
                StatusCode = (int)StatusCode.InternalServerError
            };
        }
    }

    /// <summary>
    /// deletes tag entity based on id.
    /// </summary>

    public async Task<ResponseDto> DeleteAsync(Guid id)
    {
        _logger.LogDebug("Deleting Tag with Id: {TagId}", id);

        try
        {
            var tag = await _tagRepo.GetAsync(id);
            if (tag == null)
            {
                _logger.LogWarning(" Tag with Id {TagId} not found for deletion", id);
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Tag not found",
                    StatusCode = (int)StatusCode.BadRequest
                };
            }

            var isDeleted = await _tagRepo.DeleteAsync(tag);
            if (isDeleted)
            {
                _logger.LogInformation("Tag with Id {TagId} deleted successfully", id);
                return new ResponseDto
                {
                    IsSuccess = true,
                    Message = "Tag Successfully Deleted.",
                    StatusCode = (int)StatusCode.Success
                };
            }

            _logger.LogWarning(
                " Tag with Id {TagId} not deletd, Unexpected error while ddeleting Tag.",
                id
            );

            return new ResponseDto
            {
                IsSuccess = false,
                Message = "An unexpected error occurred while deleting the Tag.",
                StatusCode = (int)StatusCode.InternalServerError
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while deleting Tag.");

            return new ResponseDto
            {
                IsSuccess = false,
                Message = "An unexpected error occurred while deleting the Tag.",
                StatusCode = (int)StatusCode.InternalServerError
            };
        }
    }

    /// <summary>
    ///fetches tag details based on Id.
    /// </summary>

    public async Task<ResponseDto> GetAsync(Guid id)
    {
        _logger.LogInformation("Fetching Tag with Id {TagId}", id);

        try
        {
            var tag = await _tagRepo.GetAsync(id);
            if (tag == null)
            {
                _logger.LogWarning(" Tag with Id {TagId} not found for deletion", id);
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Tag not found",
                    StatusCode = (int)StatusCode.BadRequest
                };
            }

            var tagDto = new TagDto
            {
                Id = tag.Id,
                IsActive = tag.IsActive,
                MacAddress = tag.MacAddress,
            };

            _logger.LogInformation("Tag {TagId} fetched successfully", id);

            return new ResponseDto<TagDto>
            {
                IsSuccess = true,
                Message = "Tags Retrived Successfully.",
                StatusCode = (int)StatusCode.Success,
                Data = tagDto
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while fetching Tag.");

            return new ResponseDto
            {
                IsSuccess = false,
                Message = "An unexpected error occurred while fetching the Tag.",
                StatusCode = (int)StatusCode.InternalServerError
            };
        }
    }

    /// <summary>
    /// fetches tags based on filter
    /// </summary>
    public async Task<ResponseDto> GetListAsync(PagedFilterRequestDto filter)
    {
        _logger.LogInformation("Fetching {MaxCount} Tags", filter.MaxResultCount);

        try
        {
            var query = _tagRepo.GetQueryable();

            filter.SearchTerm = filter.SearchTerm?.Trim().ToUpper();

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                query = query.Where(x => x.MacAddress.Contains(filter.SearchTerm));
            }

            if (!string.IsNullOrWhiteSpace(filter.SortOrder))
            {
                query = filter.SortOrder switch
                {
                    "macAddress_desc" => query.OrderByDescending(u => u.MacAddress),
                    "macAddress_asc" => query.OrderBy(x => x.MacAddress),

                    _ => query.OrderByDescending(u => u.MacAddress)
                };
            }

            var totalCount = await query.CountAsync();

            var tags = await query
                .Select(x => new TagDto
                {
                    Id = x.Id,
                    MacAddress = x.MacAddress,
                    IsActive = x.IsActive,
                })
                .Skip(filter.SkipCount)
                .Take(filter.MaxResultCount)
                .ToListAsync();

            var pagedResponse = new PagedResponseDto<TagDto>
            {
                Items = tags,
                TotalCount = totalCount,
            };

            _logger.LogInformation(
                "{AssetCount} Assets retrieved successfully",
                filter.MaxResultCount
            );

            return new ResponseDto<PagedResponseDto<TagDto>>
            {
                Data = pagedResponse,
                IsSuccess = true,
                Message = "Assets retrieved successfully",
                StatusCode = (int)StatusCode.Success,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while fetching Tags.");

            return new ResponseDto
            {
                IsSuccess = false,
                Message = "An unexpected error occurred while fetching the Tags.",
                StatusCode = (int)StatusCode.InternalServerError
            };
        }
    }

    /// <summary>
    /// updates existing Tag entity based on Id
    /// </summary>
    public async Task<ResponseDto> UpdateAsync(Guid id, UpdateTagDto input)
    {
        _logger.LogDebug("Updating Tag with Id {TagId} ", id);

        input.MacAddress = input.MacAddress.Trim().ToUpper();
        if (string.IsNullOrEmpty(input.MacAddress))
        {
            _logger.LogWarning("Tag update failed due to invalid Mac address");
            return new ResponseDto()
            {
                IsSuccess = false,
                Message = "Mac Address is required",
                StatusCode = (int)StatusCode.BadRequest,
            };
        }

        try
        {
            var tag = await _tagRepo.GetAsync(id);
            if (tag == null)
            {
                _logger.LogWarning(" Tag with Id {TagId} not found for update", id);
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Tag not found",
                    StatusCode = (int)StatusCode.NotFound,
                };
            }

            var duplicateMac = await _tagRepo
                .GetQueryable()
                .AnyAsync(x => x.Id != id && x.MacAddress == input.MacAddress);
            if (duplicateMac)
            {
                _logger.LogWarning("Mac Address {MacAddress} already exists", input.MacAddress);
                return new ResponseDto()
                {
                    IsSuccess = false,
                    Message = "Mac Address is already exists",
                    StatusCode = (int)StatusCode.BadRequest,
                };
            }

            tag.MacAddress = input.MacAddress;
            tag.IsActive = input.IsActive;

            var isUpdated = await _tagRepo.UpdateAsync(tag);
            if (isUpdated)
            {
                _logger.LogInformation("Tag with Id {TagId} updated successfully", id);
                return new ResponseDto
                {
                    IsSuccess = true,
                    Message = "Tag updated successfully",
                    StatusCode = (int)StatusCode.Success
                };
            }

            _logger.LogError("Unexpected error while updating Tag");
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "An unexpected error occurred.",
                StatusCode = (int)StatusCode.InternalServerError
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while updating Tag.");

            return new ResponseDto
            {
                IsSuccess = false,
                Message = "An unexpected error occurred while updating the Tag.",
                StatusCode = (int)StatusCode.InternalServerError
            };
        }
    }

    /// <summary>
    ///  retrives Tags data in excel format
    /// </summary>
    public async Task<(Stream? Stream, string FileName)> ExportToExcelAsync()
    {
        _logger.LogInformation("Exporting all Tags to Excel");
        try
        {
            var tags = await _tagRepo
                .GetQueryable()
                .Select(x => new TagDto
                {
                    Id = x.Id,
                    MacAddress = x.MacAddress,
                    IsActive = x.IsActive,
                })
                .ToListAsync();

            if (tags.Count < 1)
            {
                _logger.LogWarning("No assets found to export.");
                return (null, string.Empty);
            }

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Assets");

            worksheet.Cell(1, 1).Value = "Tag ID";
            worksheet.Cell(1, 2).Value = " Mac Address";
            worksheet.Cell(1, 3).Value = "Is Active";

            var dataToInsert = tags.Select(a => new object[] { a.Id, a.MacAddress, a.IsActive, });

            worksheet.Cell(2, 1).InsertData(dataToInsert);
            worksheet.Columns().AdjustToContents();

            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            var fileName = $"Tags_Export_{DateTime.UtcNow:yyyyMMdd_HHmmss}.xlsx";

            _logger.LogInformation("Successfully created Tag export file: {FileName}", fileName);

            return (stream, fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred during Tags export.");
            return (null, string.Empty);
        }
    }
}
