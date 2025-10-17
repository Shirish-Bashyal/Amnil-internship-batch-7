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
        _logger.LogInformation("Tag creation started for MAC: {MacAddress}", input.MacAddress);

        input.MacAddress = input.MacAddress.Trim().ToUpper();
        if (string.IsNullOrEmpty(input.MacAddress))
        {
            _logger.LogWarning("Tag creation failed due to missing MAC address.");
            return ResponseDto.BadRequest("Mac Address is required");
        }

        try
        {
            var duplicateMac = await _tagRepo
                .GetQueryable()
                .AnyAsync(x => x.MacAddress == input.MacAddress);
            if (duplicateMac)
            {
                _logger.LogWarning("Duplicate MAC address found: {MacAddress}", input.MacAddress);
                return ResponseDto.BadRequest("Mac Address already exists.");
            }

            var tag = new Tag { MacAddress = input.MacAddress, IsActive = true };

            var result = await _tagRepo.InsertAsync(tag);

            var createdTag = new TagDto
            {
                Id = result.Id,
                IsActive = result.IsActive,
                MacAddress = result.MacAddress,
            };

            _logger.LogInformation("Tag {MacAddress} created successfully.", input.MacAddress);

            return ResponseDto<TagDto>.Created(createdTag, "Tag created successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while creating Tag.");
            return ResponseDto.InternalServerError("An unexpected error occurred.");
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

                return ResponseDto.NotFound("Tag not found.");
            }

            var isDeleted = await _tagRepo.DeleteAsync(tag);
            if (isDeleted)
            {
                _logger.LogInformation("Tag with ID {Id} deleted successfully.", id);

                return ResponseDto.Success("Tag deleted successfully.");
            }

            _logger.LogWarning("Failed to delete Tag with ID {Id}.", id);

            return ResponseDto.InternalServerError("Failed to delete Tag.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting Tag with ID {Id}.", id);

            return ResponseDto.InternalServerError(
                "An unexpected error occurred while deleting the Tag."
            );
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
                _logger.LogWarning("Tag with ID {Id} not found.", id);
                return ResponseDto.NotFound("Tag not found.");
            }

            var tagDto = new TagDto
            {
                Id = tag.Id,
                IsActive = tag.IsActive,
                MacAddress = tag.MacAddress,
            };

            _logger.LogInformation("Tag with ID {Id} fetched successfully.", id);

            return ResponseDto<TagDto>.Success(tagDto, "Tag fetched successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching Tag with ID {Id}.", id);

            return ResponseDto.InternalServerError(
                "An unexpected error occurred while fetching the Tag."
            );
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

            _logger.LogInformation("Fetched {Count} tags successfully.", pagedResponse.TotalCount);
            return ResponseDto<PagedResponseDto<TagDto>>.Success(
                pagedResponse,
                "Tags fetched successfully."
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching tags.");

            return ResponseDto.InternalServerError(
                "An unexpected error occurred while fetching tags."
            );
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

            return ResponseDto.BadRequest("Mac Address is required.");
        }

        try
        {
            var tag = await _tagRepo.GetAsync(id);
            if (tag == null)
            {
                _logger.LogWarning("Tag with ID {Id} not found.", id);

                return ResponseDto.NotFound("Tag not found.");
            }

            var duplicateMac = await _tagRepo
                .GetQueryable()
                .AnyAsync(x => x.Id != id && x.MacAddress == input.MacAddress);
            if (duplicateMac)
            {
                _logger.LogWarning("Mac Address {MacAddress} already exists", input.MacAddress);

                return ResponseDto.BadRequest("Mac Address already exists.");
            }

            tag.MacAddress = input.MacAddress;
            tag.IsActive = input.IsActive;

            var isUpdated = await _tagRepo.UpdateAsync(tag);
            if (isUpdated)
            {
                _logger.LogInformation("Tag with Id {TagId} updated successfully", id);
                return ResponseDto.Success("Tag updated successfully");
            }

            _logger.LogError("Unexpected error while updating Tag");
            return ResponseDto.InternalServerError("An unexpected error occurred.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while updating Tag");
            return ResponseDto.InternalServerError("An unexpected error occurred.");
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
