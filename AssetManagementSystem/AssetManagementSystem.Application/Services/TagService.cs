using AssetManagementSystem.Contract.Interfaces;
using AssetManagementSystem.Contract.Interfaces.Service;
using AssetManagementSystem.Entity.Entities;
using AssetManagementSystem.Shared.Dtos;
using AssetManagementSystem.Shared.Dtos.Tag;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using static AssetManagementSystem.Shared.Constrains.EntityConstrains;

namespace AssetManagementSystem.Application.Services;

public class TagService : ITagService
{
    private readonly IGenericRepository<TagModel> _tagRepo;
    private readonly ILogger<TagService> _logger;
    public TagService(IGenericRepository<TagModel> tagRepo, ILogger<TagService> logger)
    {
        _tagRepo = tagRepo;
        _logger = logger;
    }
    public async Task<ResponseData<Guid>> CreateAsync(TagDto dto)
    {
        _logger.LogInformation("CreateAsync called for MacAddress: {MacAddress}", dto.MacAddress);

        if (string.IsNullOrWhiteSpace(dto.MacAddress)|| string.IsNullOrWhiteSpace(dto.Description))
        {
            _logger.LogWarning("CreateAsync failed due to empty MacAddress or Description");
            return ResponseData<Guid>.BadRequest("Fields should not be empty");
            
        }
        try
        {
            var checkExistance = await _tagRepo
                                .GetQueryable()
                                .AnyAsync(u => u.MacAddress == dto.MacAddress);


            if (checkExistance)
            {
                _logger.LogWarning("Tag with MacAddress {MacAddress} already exists", dto.MacAddress);
                return ResponseData<Guid>.BadRequest("Mac Address already present");
                
            }

            var tag = new TagModel
            {
                MacAddress = dto.MacAddress.Trim(),
                Description = dto.Description.Trim(),
                CreatedAt = DateTime.UtcNow,
                AssetId = dto.AssetId,
                IsActive = true,
            };

            var result = await _tagRepo.CreateAsync(tag);
            _logger.LogInformation("Tag created successfully with Id: {Id}", result.Id);
            return ResponseData<Guid>.Success("Tag Sucessfully Created");
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "Error occurred while creating Tag with MacAddress {MacAddress}", dto.MacAddress);
            return ResponseData<Guid>.Exception("Unexpected Error Occured");
        }
    }

    public async Task<ResponseData<bool>> DeleteAsync(Guid id)
    {
        _logger.LogInformation("DeleteAsync called for Tag Id: {Id}", id);
        try
        {
            var tag = await _tagRepo.GetAsync(id);

            if (tag == null || tag.IsActive == false)
            {
                _logger.LogWarning("Tag with Id {Id} not found or inactive", id);
                return ResponseData<bool>.BadRequest("Tag not Found");
               
            }

            tag.IsActive = false;

            var result = await _tagRepo.DeleteAsync(tag);

            if (result)
            {
                _logger.LogInformation("Tag with Id {Id} deleted successfully", id);
                return ResponseData<bool>.Success("Tag Sucessfully Deleted");
            }

            _logger.LogWarning("Failed to delete Tag with Id {Id}", id);
            return ResponseData<bool>.BadRequest("Tag not Found");
           
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "Error occurred while deleting Tag with Id {Id}", id);
            return ResponseData<bool>.Exception("Unexpected Error Occured");
        }
        

    }

    public async Task<ResponseData<IEnumerable<TagReadDto>>> GetAllAsync()
    {
        _logger.LogInformation("GetAllAsync called for Tags");
        try
        {
            //var tags = await _tagRepo.GetAllAsync();

            var tags = await _tagRepo.GetQueryable()
                                 .Include(t => t.Asset)
                                 .ToListAsync();

            if (tags == null)
            {
                _logger.LogWarning("No tags found");
                return ResponseData<IEnumerable<TagReadDto>>.BadRequest("Tag not found");
            }

           
            var tagDto = tags.Select(t => new TagReadDto
            {
                Id = t.Id,
                MacAddress = t.MacAddress,
                CreatedAt = t.CreatedAt,
                AssetId = t.AssetId,
                AssetName = t.Asset?.AssetName,  // populate from navigation
                Description = t.Description,
                IsActive = t.IsActive
            });

            return new ResponseData<IEnumerable<TagReadDto>>
            {
                IsSuccess = true,
                Data = tagDto
            };
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "Error occurred while fetching all Tags");
            return ResponseData<IEnumerable<TagReadDto>>.Exception("Unexpected Error Occured");
        }
        
    }

    public async Task<ResponseData<TagReadDto>> GetAsync(Guid id)
    {
        _logger.LogInformation("GetAsync called for Tag Id: {Id}", id);
        try
        {
            //var tag=await _tagRepo.GetAsync(id);

            var tag = await _tagRepo.GetQueryable()
                                 .Include(t => t.Asset)
                                 .FirstOrDefaultAsync(t => t.Id == id);

            if (tag == null || tag.IsActive == false)
            {
                _logger.LogWarning("Tag with Id {Id} not found or inactive", id);
                return ResponseData<TagReadDto>.BadRequest("Tag not Found");
            }


            var tagDto = new TagReadDto
            {
                Id = tag.Id,
                MacAddress = tag.MacAddress,
                CreatedAt = tag.CreatedAt,
                AssetId = tag.AssetId,
                AssetName = tag.Asset?.AssetName,  
                Description = tag.Description,
                IsActive = tag.IsActive,
            };

            return new ResponseData<TagReadDto>
            {
                IsSuccess = true,
                Data = tagDto
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching Tag with Id {Id}", id);
            return ResponseData<TagReadDto>.Exception("Unexpected Error Occured");
        }
    }

    public async Task<ResponseData<bool>> UpdateAsync(Guid id, TagDto dto)
    {
        _logger.LogInformation("UpdateAsync called for Tag Id: {Id}", id);

        try
        {
            var tag = await _tagRepo.GetAsync(id);

            if (tag == null)
            {
                _logger.LogWarning("Tag with Id {Id} not found", id);
                return ResponseData<bool>.BadRequest("Tag not Found");
                
            }

            tag.MacAddress= dto.MacAddress.Trim();
            tag.ModifiedAt = DateTime.UtcNow;
            tag.Description= dto.Description.Trim();
            tag.AssetId=dto.AssetId;

            var result = await _tagRepo.UpdateAsync(tag);

            if (result)
            {
                _logger.LogInformation("Tag with Id {Id} updated successfully", id);
                return ResponseData<bool>.Success("Tag Updated Sucessfully");
                return new ResponseData<bool>
                {
                    IsSuccess = true,
                    Message = "Tag Updated Sucessfully"
                };
            }
            _logger.LogWarning("Failed to update Tag with Id {Id}", id);
            return ResponseData<bool>.BadRequest("Tag not Found");
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "Error occurred while updating Tag with Id {Id}", id);
            return ResponseData<bool>.Exception("Unexpected Error Occured");
        }
    }
    }

