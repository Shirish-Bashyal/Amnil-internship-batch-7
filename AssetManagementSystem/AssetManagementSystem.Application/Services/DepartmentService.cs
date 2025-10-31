using AssetManagementSystem.Contract.Interfaces;
using AssetManagementSystem.Contract.Interfaces.Service;
using AssetManagementSystem.Entity.Entities;
using AssetManagementSystem.Shared.Dtos;
using AssetManagementSystem.Shared.Dtos.Department;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mapster;

namespace AssetManagementSystem.Application.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IGenericRepository<DepartmentModel> _departmentRepo;
    private readonly ILogger<DepartmentService> _logger;
    public DepartmentService(IGenericRepository<DepartmentModel> departmentRepo, ILogger<DepartmentService> logger)
    {
        _departmentRepo = departmentRepo;
        _logger = logger;
    }


    public async Task<ResponseData<Guid>> CreateAsync(DepartmentDto dto)
    {
        _logger.LogInformation("CreateAsync called for Department Name: {Name}", dto.Name);

        if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Description))
        {
            _logger.LogWarning("CreateAsync failed due to empty Name or Description");

            return ResponseData<Guid>.BadRequest("Name and Description cannot be empty");

        }

        try
        {

            var checkExistance = await _departmentRepo
                .GetQueryable()
                .AnyAsync(u => u.Name == dto.Name);

            if (checkExistance)
            {
                _logger.LogWarning("Department with Name {Name} already exists", dto.Name);
                return ResponseData<Guid>.BadRequest("Department Name already present");
                
            }

            var department = new DepartmentModel
            {
                Name = dto.Name.Trim(),
                Description = dto.Description.Trim(),
                CreatedAt = DateTime.UtcNow,
                IsActive=true,
            };

            var result = await _departmentRepo.CreateAsync(department);

            _logger.LogInformation("Department created successfully with Id: {Id}", result.Id);
            return ResponseData<Guid>.Success("Department added successfully");

           
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating department {Name}", dto.Name);
            return ResponseData<Guid>.Exception("Unexpected Error Occured");
        }

        
    }

    public async Task<ResponseData<bool>> DeleteAsync(Guid id)
    {
        _logger.LogInformation("DeleteAsync called for Department Id: {Id}", id);
        try
        {
            var department = await _departmentRepo.GetAsync(id);

            if (department == null)
            {
                _logger.LogWarning("Department with Id {Id} not found", id);

                return ResponseData<bool>.BadRequest("Department Not Found");
            }

            department.IsActive = false;

            var isDeleted = await _departmentRepo.DeleteAsync(department);

            if (isDeleted)
            {
                _logger.LogInformation("Department with Id {Id} deleted successfully", id);
                return ResponseData<bool>.Success("Department Deleted Sucessfully");
               
            }
            _logger.LogWarning("Failed to delete department with Id {Id}", id);
            return ResponseData<bool>.BadRequest("Department Not Found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting department with Id {Id}", id);
            return ResponseData<bool>.Exception("Unexpected Error Occured");
        }

    }

    public async Task<ResponseData<IEnumerable<DepartmentReadDto>>> GetAllAsync()
    {
        _logger.LogInformation("GetAllAsync called");

        try
        {
            var departments=await _departmentRepo.GetAllAsync();

            if (departments == null)
            {
                _logger.LogWarning("No departments found");

                return ResponseData<IEnumerable<DepartmentReadDto>>.Success("No Data Found");
                

            }



            var DepartmentDto = departments.Select(x => x.Adapt<DepartmentReadDto>()).ToList();



            //    departments.Select(u => new DepartmentReadDto
            //{
            //    Id = u.Id,
            //    Name = u.Name,
            //    Description=u.Description,
            //    CreatedAt=u.CreatedAt,
            //    IsActive=u.IsActive,
            //});


            return new ResponseData<IEnumerable<DepartmentReadDto>>
            {
                Data = DepartmentDto,
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching all departments");
            return ResponseData<IEnumerable<DepartmentReadDto>>.Exception("Unexpected Error Occured");
        }
    }

    public async Task<ResponseData<DepartmentReadDto>> GetAsync(Guid id)
    {
        _logger.LogInformation("GetAsync called for Department Id: {Id}", id);

        try
        {

            var department=await _departmentRepo.GetAsync(id);

            if (department == null || !department.IsActive)
            {
                _logger.LogWarning("Department with Id {Id} not found", id);
                return ResponseData<DepartmentReadDto>.BadRequest("Department Not Found");
                
            }

            var DepartmentDto = department.Adapt<DepartmentReadDto>();

            //var DepartmentDto =  new DepartmentReadDto
            //{
            //    Id = department.Id,
            //    Name = department.Name,
            //    Description= department.Description,
            //    CreatedAt= department.CreatedAt,

            //};

            return new ResponseData<DepartmentReadDto>
            {
                Data = DepartmentDto,
                IsSuccess = true
            };

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching department with Id {Id}", id);
            return ResponseData<DepartmentReadDto>.Exception("Unexpected Error Occured");
        }
    }

    public async Task<ResponseData<bool>> UpdateAsync(Guid id, DepartmentDto dto)
    {
        _logger.LogInformation("UpdateAsync called for Department Id: {Id}", id);

        try
        {
            var department = await _departmentRepo.GetAsync(id);

            if (department == null || !department.IsActive)
            {
                _logger.LogWarning("Department with Id {Id} not found", id);

                return ResponseData<bool>.BadRequest("Department Not Found");
            }

            department.Name = dto.Name;
            department.Description = dto.Description;
            department.ModifiedAt = DateTime.UtcNow;

            var isUpdated = await _departmentRepo.UpdateAsync(department);

            if (isUpdated)
            {
                _logger.LogInformation("Department with Id {Id} updated successfully", id);
                return ResponseData<bool>.Success("Department  updated successfully");
            }
            _logger.LogWarning("Failed to update department with Id {Id}", id);

            return ResponseData<bool>.BadRequest("Department not Updated");
            
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating department with Id {Id}", id);
            return ResponseData<bool>.Exception("Unexpected Error Occured");

        }
    }


}
