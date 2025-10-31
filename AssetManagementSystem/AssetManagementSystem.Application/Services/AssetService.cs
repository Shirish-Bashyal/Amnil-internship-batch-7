using AssetManagementSystem.Contract.Interfaces;
using AssetManagementSystem.Contract.Interfaces.Service;
using AssetManagementSystem.Entity.Entities;
using AssetManagementSystem.Shared;
using AssetManagementSystem.Shared.Dtos;
using AssetManagementSystem.Shared.Dtos.Asset;
using ClosedXML.Excel;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;



namespace AssetManagementSystem.Application.Services;

public class AssetService : IAssetService
{
    private readonly IGenericRepository<AssetModel> _assetRepo;
    private readonly IGenericRepository<DepartmentModel> _departmentRepo;
    private readonly ILogger<AssetService> _logger;

    public AssetService(IGenericRepository<AssetModel> assetRepo, ILogger<AssetService> logger)
    {
        _assetRepo = assetRepo;
        _logger = logger;
    }
    public async Task<ResponseData<Guid>> CreateAsync(AssetDto dto)
    {
        _logger.LogInformation("CreateAsync called for AssetName: {AssetName}", dto.AssetName);

        if (string.IsNullOrWhiteSpace(dto.AssetName) ||
            string.IsNullOrWhiteSpace(dto.SerialNumber) ||
            string.IsNullOrWhiteSpace(dto.AssetCategory) ||
            string.IsNullOrWhiteSpace(dto.Description) ||
            dto.DepartmentId == Guid.Empty
            )


        {
            _logger.LogWarning("CreateAsync failed due to missing required fields");

            return ResponseData<Guid>.BadRequest("Please fill in all required fields: Asset Name, Serial Number, Asset Category, and Department.");

           
        }

        try
        {

            var checkExistance = await _assetRepo
                .GetQueryable()
                .AnyAsync(u => u.SerialNumber == dto.SerialNumber);

            if (checkExistance)
            {
                _logger.LogWarning("Asset with SerialNumber {SerialNumber} already exists", dto.SerialNumber);

                return ResponseData<Guid>.BadRequest("Asset Already Present");

               
            }

           
            var asset = new AssetModel
            {
                AssetName = dto.AssetName.Trim(),
                Description = dto.Description.Trim(),
                AssetCategory = dto.AssetCategory.Trim(),
                SerialNumber = dto.SerialNumber.Trim(),
                DepartmentId = dto.DepartmentId,
                ReceivedDate = dto.ReceivedDate == default ? DateTime.UtcNow : dto.ReceivedDate,
                IsActivated=true,
                CreatedAt = DateTime.Now,
                IsActive = true,
            };

            var result = await _assetRepo.CreateAsync(asset);
            _logger.LogInformation("Asset created successfully with Id: {Id}", result.Id);

            return ResponseData<Guid>.Success("Asset Added Successfully");

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating asset");
            return ResponseData<Guid>.Exception("An unexpected error occurred.");
        }
    }

    public async Task<ResponseData<bool>> DeleteAsync(Guid id)
    {
        _logger.LogInformation("DeleteAsync called for AssetId: {Id}", id);

        try
        {
            var asset = await _assetRepo.GetAsync(id);
            
            if (asset == null || asset.IsActive == false)
            {
                _logger.LogWarning("Asset with Id {Id} not found or already inactive", id);

                return ResponseData<bool>.BadRequest("Asset Not Found");

            }

            asset.IsActive = false;

            var result = await _assetRepo.DeleteAsync(asset);

            if (result)
            {
                _logger.LogInformation("Asset with Id {Id} soft-deleted successfully", id);

                return ResponseData<bool>.Success("Asset Deleted Sucessfully");
            }

            _logger.LogWarning("Asset with Id {Id} could not be deleted", id);
            return ResponseData<bool>.BadRequest("Asset Not Found");
           
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting asset with Id {Id}", id);
            return ResponseData<bool>.Exception("Unexpected Error Occured");
        }

    }

    public async Task<ResponseData<IEnumerable<AssetReadDto>>> GetAllAsync()
    {
        _logger.LogInformation("GetAllAsync called");
        try
        {
            //var assets = await _assetService.GetAllAsync();

            var assets = await _assetRepo.GetQueryable()
                                     .Include(a => a.Department) 
                                     .Include(a => a.Tag)        
                                     .ToListAsync();

            if (assets == null)
            {
                _logger.LogWarning("No assets found");
                return ResponseData<IEnumerable<AssetReadDto>>.BadRequest("Asset Not Found");


            }


            var assetDtos = assets.Select(x => x.Adapt<AssetReadDto>()).ToList();

            //    a => new AssetReadDto
            //{
            //    Id=a.Id,
            //    AssetName = a.AssetName,
            //    SerialNumber = a.SerialNumber,
            //    AssetCategory = a.AssetCategory,
            //    ReceivedDate = a.ReceivedDate,
            //    IsActive = a.IsActive,
            //    DepartmentId = a.DepartmentId,
            //    DepartmentName = a.Department?.Name, 
            //    TagId = a.Tag?.Id,
            //    IsActivated= a.IsActivated,
            //    TagName = a.Tag?.MacAddress,        
            //    Description = a.Description
            //});

            return new ResponseData<IEnumerable<AssetReadDto>>
            {
                IsSuccess = true,
                Data = assetDtos

            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all assets");
            return ResponseData<IEnumerable<AssetReadDto>>.Exception("Unexpected Error Occured");

        }

    }


    public async Task<ResponseData<AssetReadDto>> GetAsync(Guid id)
    {
        _logger.LogInformation("GetAsync called for AssetId: {Id}", id);
        try
        {
            //var asset = await _assetService.GetAsync(id);

            var asset = await _assetRepo.GetQueryable()
                                     .Include(a => a.Department)
                                     .Include(a => a.Tag)
                                     .FirstOrDefaultAsync(a => a.Id == id);

            if (asset == null || asset.IsActive == false)
            {
                _logger.LogWarning("Asset with Id {Id} not found", id);
                return ResponseData<AssetReadDto>.BadRequest("Asset Not Found");
                
            }


            var assetDto = asset.Adapt<AssetReadDto>();


            //new AssetReadDto()
            //{
            //    Id = asset.Id,
            //    AssetName = asset.AssetName,
            //    AssetCategory = asset.AssetCategory,
            //    CreatedAt = asset.CreatedAt,
            //    ReceivedDate = asset.ReceivedDate,
            //    DepartmentId = asset.DepartmentId,
            //    DepartmentName = asset.Department?.Name, 
            //    SerialNumber = asset.SerialNumber,
            //    Description = asset.Description,
            //    IsActivated = asset.IsActivated,
            //    TagId = asset.Tag?.Id,                   
            //    TagName = asset.Tag?.MacAddress          
            //};


            return new ResponseData<AssetReadDto>
            {
                IsSuccess = true,
                Data = assetDto

            };

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving asset with Id {Id}", id);
            return ResponseData<AssetReadDto>.Exception("Unexpected Error Occured");
        }

    }

    public async Task<ResponseData<bool>> UpdateAsync(Guid id, AssetDto dto)
    {
        _logger.LogInformation("UpdateAsync called for AssetId: {Id}", id);
        try
        {
            var asset = await _assetRepo.GetAsync(id);

            if (asset == null)
            {
                _logger.LogWarning("Asset with Id {Id} not found", id);
                return ResponseData<bool>.BadRequest("Asset Not Found");

            }

            asset.AssetName = dto.AssetName.Trim();
            asset.AssetCategory = dto.AssetCategory.Trim();
            asset.SerialNumber = dto.SerialNumber.Trim();
            asset.Description = dto.Description.Trim();
            asset.IsActivated = dto.IsActivated;
            asset.DepartmentId = dto.DepartmentId;
            asset.ReceivedDate = dto.ReceivedDate;


            var result = await _assetRepo.UpdateAsync(asset);

            if (result)
            {
                _logger.LogInformation("Asset with Id {Id} updated successfully", id);
                return ResponseData<bool>.Success ("Asset Updated Sucessfully" );
            }
            _logger.LogWarning("Failed to update asset with Id {Id}", id);
            return ResponseData<bool>.BadRequest("Asset Not Found");
        }
        catch (Exception ex) 
        {

            _logger.LogError(ex, "Error occurred while updating asset with Id {Id}", id);
            return ResponseData<bool>.Exception("Unexpected Error Occured");

        }
        

    }

    public async Task<ResponseData<bool>> ChangeActivationStatusAsync(Guid id, bool isActivated)
    {
        _logger.LogInformation("ChangeActivationStatusAsync called for AssetId: {Id}, New Status: {Status}", id, isActivated);

        try
        {
            //var asset = await _assetService.GetAsync(id);

            var asset = await _assetRepo.GetQueryable()
                        .Include(a => a.Tag)
                        .FirstOrDefaultAsync(a => a.Id == id);


            if (asset == null || asset.IsActive == false)
            {
                _logger.LogWarning("Asset with Id {Id} not found or inactive", id);
                return ResponseData<bool>.BadRequest("Asset Not Found");
               
            }

            if (isActivated && asset.Tag == null)
            {
                _logger.LogWarning("Attempted to deactivate asset {Id} without Tag assigned", id);
                return ResponseData<bool>.BadRequest("Cannot activate asset without a Tag assigned");
                
            }

            asset.IsActivated = isActivated;

            var result = await _assetRepo.UpdateAsync(asset);

            if (result)
            {
                _logger.LogInformation("Asset activation status updated successfully for Id: {Id}", id);
                return ResponseData<bool>.Success($"Asset {(isActivated ? "Activated" : "Deactivated")} Successfully)");
                
            }

            _logger.LogWarning("Failed to update activation status for Asset with Id {Id}", id);
            return ResponseData<bool>.BadRequest("Failed to Update Asset Activation Status");
            
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while changing activation status for AssetId: {Id}", id);
            return ResponseData<bool>.Exception("Unexpected Error Occured");
        }
    }



    public async Task<(Stream? Stream, string FileName)> ExportToExcelAsync()
    {
        _logger.LogInformation("Exporting all Assets to Excel");

        try
        {
            var assets = await _assetRepo
           .GetQueryable()
           .Include(a => a.Department) // join Department table
           .Include(a => a.Tag)        // join Tag table (optional)
           .Select(x => new
           {
               x.Id,
               x.AssetName,
               x.SerialNumber,
               x.AssetCategory,
               x.ReceivedDate,
               x.IsActive,
               x.IsActivated,
               DepartmentId = x.Department != null ? x.Department.Id : Guid.Empty,
               DepartmentName = x.Department != null ? x.Department.Name : string.Empty,
               MacAddress = x.Tag!=null ? x.Tag.MacAddress:string.Empty,
              
           }).ToListAsync();

            if (!assets.Any())
            {
                _logger.LogWarning("No assets found to export.");
                return (null, string.Empty);
            }

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Assets");

            // Header row
            worksheet.Cell(1, 1).Value = "Asset ID";
            worksheet.Cell(1, 2).Value = "Asset Name";
            worksheet.Cell(1, 3).Value = "Serial Number";
            worksheet.Cell(1, 4).Value = "Asset Category";
            worksheet.Cell(1, 5).Value = "Received Date";
            worksheet.Cell(1, 6).Value = "Is Active";
            worksheet.Cell(1, 7).Value = "Is Activated";
            worksheet.Cell(1, 8).Value = "Department ID";
            worksheet.Cell(1, 9).Value = "Department Name";
            worksheet.Cell(1, 10).Value = "MAC Address";
            

            // Data rows
            var dataToInsert = assets.Select(a => new object[]
            {
            a.Id,
            a.AssetName,
            a.SerialNumber,
            a.AssetCategory,
            a.ReceivedDate,
            a.IsActive,
            a.IsActivated,
            a.DepartmentId,
            a.DepartmentName ?? string.Empty,
            a.MacAddress ?? string.Empty
            });

            worksheet.Cell(2, 1).InsertData(dataToInsert);
            worksheet.Columns().AdjustToContents();
            worksheet.Column(5).Style.DateFormat.SetFormat("yyyy-MM-dd");

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

    public async Task<ResponseData<PagedResponseDto<AssetReadDto>>> GetListAsync(PaginationAndFilterRequest filter)
    {
        _logger.LogInformation(
            "Fetching {AssetCount} Asset with pagination",
            filter.MaxResultCount
        );

        try
        {
            var query = _assetRepo.GetQueryable();


            query = query.Include(a => a.Department);

            query = query.Include(a => a.Tag);

            var searchTerm = filter.SearchTerm?.Trim().ToUpper();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x =>
                    x.AssetName.ToUpper().Contains(searchTerm) ||
                    x.SerialNumber.ToUpper().Contains(searchTerm) ||
                    x.AssetCategory.ToUpper().Contains(searchTerm) ||
                    (x.Department != null && x.Department.Name.ToUpper().Contains(searchTerm)) ||
                    (x.Tag != null && x.Tag.MacAddress.ToUpper().Contains(searchTerm))
                );
            }



            if (!string.IsNullOrWhiteSpace(filter.SortOrder))
            {
                query = filter.SortOrder switch
                {
                    "department_desc" => query.OrderByDescending(x => x.Department.Name),
                    "department_asc" => query.OrderBy(x => x.Department.Name),
                    "name_desc" => query.OrderByDescending(u => u.AssetName),
                    "name_asc" => query.OrderBy(u => u.AssetName),
                    "date_asc" => query.OrderBy(u => u.ReceivedDate),
                    "date_desc" => query.OrderByDescending(u => u.ReceivedDate),
                    "serialNumber_desc" => query.OrderByDescending(x => x.SerialNumber),
                    "serialNumber_asc" => query.OrderBy(x => x.SerialNumber),
                    "category_desc" => query.OrderByDescending(x => x.AssetCategory),
                    "category_asc" => query.OrderBy(x => x.AssetCategory),
                    _ => query.OrderByDescending(u => u.ReceivedDate)
                };
            }

            var totalCount = await query.CountAsync();

            // Pagination
            var assets = await query

                .Select(x => x.Adapt<AssetReadDto>())
                
                //x => new AssetReadDto
                //{
                //    Id = x.Id,
                //    AssetName = x.AssetName,
                //    AssetCategory = x.AssetCategory,
                //    CreatedAt = x.CreatedAt,
                //    ReceivedDate = x.ReceivedDate,
                //    DepartmentId = x.DepartmentId,
                //    DepartmentName = x.Department != null ? x.Department.Name : null,
                //    SerialNumber = x.SerialNumber,
                //    Description = x.Description,
                //    IsActivated = x.IsActivated,
                //    TagId = x.Tag != null ? x.Tag.Id : (Guid?)null,
                //    TagName = x.Tag != null ? x.Tag.MacAddress : null


                //})

                .Skip(filter.SkipCount)
                .Take(filter.MaxResultCount)
                .ToListAsync();



            _logger.LogInformation(
                "{AssetCount} Assets retrieved successfully",
                assets.Count
            );

            return new ResponseData<PagedResponseDto<AssetReadDto>>
            {
                IsSuccess = true,
                Data = new PagedResponseDto<AssetReadDto>
                {
                    Items = assets,
                    TotalCount = totalCount
                },
                Message = "Assets retrieved successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while fetching asset.");

            return ResponseData<PagedResponseDto<AssetReadDto>>.Exception("An unexpected error occurred");

        }
    }

    public async Task<ResponseData<List<AssetCountDto>>> GetDepartmentData()
    {

        try
        {


            var groupedData = _assetRepo.GetQueryable()
                              .Include(a => a.Department)
                              .GroupBy(d => new
                                {
                                    d.DepartmentId,
                                    DepartmentName =  d.Department.Name ,

                                });


            var finaldata= await groupedData

                                .Select(g => new AssetCountDto
                                {
                                    DepartmentId = g.Key.DepartmentId,  
                                    DepartmentName = g.Key.DepartmentName,


                                    Assets = g.Select(x => x.Adapt<BasicAssetDto>()).ToList(),

                                    //Assets = g.Select(x => new BasicAssetDto
                                    //{
                                        
                                    //    SerialNumber = x.SerialNumber,
                                    //    AssetName = x.AssetName,
                                       
                                    //}).ToList(),

                                    AssetCount = g.Count()

                                })

                                .ToListAsync();

                        return new ResponseData<List<AssetCountDto>>
                        {
                            IsSuccess = true,
                            Data = finaldata

                        };
        }
        catch
        {
            return new ResponseData<List<AssetCountDto>>
            {
                IsSuccess = true,
                Message="An error occured"

            };
        }

        
    }

}
