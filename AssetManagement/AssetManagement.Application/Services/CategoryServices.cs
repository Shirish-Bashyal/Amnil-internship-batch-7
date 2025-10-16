using AssetManagement.Contract.Repository;
using AssetManagement.Contract.Service;
using AssetManagement.Domain.Dtos;
using AssetManagement.Domain.Dtos.AssetDto;
using AssetManagement.Domain.Entity.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AssetManagement.Application.Services;

public class CategoryServices : ICategoryService
{
    private readonly IGenericRepository<Category> _categoryRepo;
    private readonly ILogger<Category> _logger;
    public CategoryServices(IGenericRepository<Category> categoryRepo, ILogger<Category> logger)
    {
        _categoryRepo = categoryRepo;
        _logger = logger;
    }

    public async Task<ServiceResponseDto<Guid>> CreateAsync(CategoryDto categoryDto)
    {
        if (categoryDto == null)
        {
            _logger.LogWarning("The Dto is Empty");
            return new ServiceResponseDto<Guid> { Message = "No Data in Dto", Success = false };
        }
        try
        {
            var exist = await _categoryRepo.GetQueryable().AnyAsync(x => x.CategoryName == categoryDto.CategoryName);
            if (exist)
            {
                _logger.LogWarning("Existing Category");
                return new ServiceResponseDto<Guid>
                {
                    Success = false,
                    Message = "Existing Category"
                };
            }
            var category = new Category { Id = Guid.NewGuid(), CategoryName = categoryDto.CategoryName };
            await _categoryRepo.InsertAsync(category);
            _logger.LogInformation("Category Created Successfully");
            return new ServiceResponseDto<Guid> { Message = "Category Created Successfully", Success = true, Data = category.Id };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occred");
            throw;
        }
    }


    public async Task<ServiceResponseDto<IEnumerable<CategoryDto>>> GetAllAsync()
    {
        try
        {
            var categoryList = await _categoryRepo.GetAllAsync();
            if (categoryList == null)
            {
                _logger.LogInformation("No Category Avilable");
                return new ServiceResponseDto<IEnumerable<CategoryDto>> { Message = "No Asset", Success = true, Data = null };
            }
            var category = categoryList.Select(x => new CategoryDto
            {
                Id = x.Id,
                CategoryName = x.CategoryName
            });
            _logger.LogInformation("Inforamtion Successfully Retrived");
            return new ServiceResponseDto<IEnumerable<CategoryDto>> { Message = " Asset Retrived", Success = true, Data = category };

        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Error occured");
            throw;
        }
    }
    public Task<ServiceResponseDto<bool>> DeleteAsync(CategoryDto categoryDto)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResponseDto<CategoryDto>> GetAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResponseDto<IEnumerable<AssetDto>>> GetPagination(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResponseDto<bool>> UpdateAsync(CategoryDto categoryDto)
    {
        throw new NotImplementedException();
    }
}
