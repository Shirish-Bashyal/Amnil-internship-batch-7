using AssetManagementSystem.Client.Models;
using AssetManagementSystem.Client.Models.Assets;
using AssetManagementSystem.Client.Models.Tag;
using AssetManagementSystem.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AssetManagementSystem.Client.Controllers;

public class AssetController : Controller
{
    private readonly IHttpClientFactory _clientFactory;

    public AssetController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    /// <summary>
    /// returns create view
    /// </summary>


    public IActionResult Create()
    {
        PopulateDropdowns();

        return View();
    }

    /// <summary>
    /// adds a new asset
    /// </summary>

    [HttpPost]
    public async Task<IActionResult> Create(AssetModel asset)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Error = "Please provide valid data";

            return View();
        }

        var client = _clientFactory.CreateClient("AssetManagementApi");
        var jsonContent = System.Text.Json.JsonSerializer.Serialize(asset);
        var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
        var response = await client.PostAsync("asset", content);

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "Asset Creation Failed";

            PopulateDropdowns();
            return View(asset);
        }
        TempData["Success"] = "Asset created successfully";
        return RedirectToAction(nameof(Index));
    }

    private void PopulateDropdowns()
    {
        //fetch all departments and categories from database
        //using static data for now

        var departments = new[]
        {
            new { Id = "11111111-1111-1111-1111-111111111111", Name = "Finance" },
            new { Id = "11111111-1111-1111-6711-111111111111", Name = "IT" },
            new { Id = "11987611-1111-1111-1111-111111111111", Name = "HR" }
        };

        var categories = new[]
        {
            new { Id = "11111111-1111-1111-1111-111111402111", Name = "Vehicles" },
            new { Id = "22222222-2222-2222-2222-222222222222", Name = "Electronics" },
            new { Id = "33333333-3333-3333-3333-333333333333", Name = "Software" }
        };

        ViewBag.Departments = new SelectList(departments, "Id", "Name");
        ViewBag.Categories = new SelectList(categories, "Id", "Name");
    }

    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    ///fetches Asset based on given filters
    /// </summary>

    [HttpGet]
    public async Task<IActionResult> Index(
        string searchString,
        int pageNumber,
        int pageSize,
        string sortOrder,
        string? category,
        bool? status
    )
    {
        if (string.IsNullOrEmpty(sortOrder))
        {
            sortOrder = "date_desc";
            TempData["CreatedAtSortParam"] = "date_desc";
        }

        TempData["NameSortParam"] = sortOrder == "name_desc" ? "name_asc" : "name_desc";
        TempData["SerialNumberSortParam"] =
            sortOrder == "serialNumber_desc" ? "serialNumber_asc" : "serialNumber_desc";
        TempData["DescriptionSortParam"] =
            sortOrder == "description_desc" ? "description_asc" : "description_desc";
        TempData["CategorySortParam"] =
            sortOrder == "category_desc" ? "category_asc" : "category_desc";
        TempData["DepartmentSortParam"] =
            sortOrder == "department_desc" ? "department_asc" : "department_desc";
        TempData["CreatedAtSortParam"] = sortOrder == "date_desc" ? "date_asc" : "date_desc";

        TempData["CurrentSort"] = sortOrder;

        TempData["CurrentFilter"] = searchString;

        TempData["SelectedCategory"] = category;
        TempData["Status"] = status;

        Console.WriteLine(status);

        PopulateDropdowns();

        if (pageSize < 5)
            pageSize = 5;

        if (pageNumber < 1)
            pageNumber = 1;
        int skipCount = (pageNumber - 1) * pageSize;

        var client = _clientFactory.CreateClient("AssetManagementApi");

        var response = await client.GetAsync(
            $"asset/list?SearchTerm={searchString}&SkipCount={skipCount}&MaxResultCount={pageSize}&SortOrder={sortOrder}&CategoryId={category}&IsActive={status}"
        );
        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "Unable to fetch Assets.";
            return View();
        }
        var result = await response.Content.ReadFromJsonAsync<
            ResponseDto<PagedResponseDto<AssetViewModel>>
        >();

        if (result == null || !result.IsSuccess || result.Data == null)
        {
            ViewBag.Error = result?.Message;
            return View(new List<AssetViewModel>());
        }
        //viewmodel
        var PagedResult = new PagedViewModel<AssetViewModel>
        {
            TotalItems = result.Data.TotalCount,
            TotalPages = (int)Math.Ceiling((double)result.Data.TotalCount / pageSize),
            PageIndex = pageNumber,
            PageSize = pageSize,
            Items = result.Data.Items,
        };

        return View(PagedResult);
    }

    /// <summary>
    /// changes status of the asset
    /// </summary>

    [HttpPost]
    public async Task<IActionResult> ToggleActive(
        Guid id,
        string newStatus,
        string? searchString,
        int pageNumber = 1,
        int pageSize = 10,
        string? sortOrder = null
    )
    {
        Console.WriteLine($"{newStatus}");

        var isActive = false;
        if (newStatus == "Active")
        {
            isActive = true;
        }
        Console.WriteLine($"{isActive}");
        var client = _clientFactory.CreateClient("AssetManagementApi");
        var response = await client.PatchAsync($"asset/{id}/{isActive}", null);
        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Error";
            return RedirectToAction(
                "Index",
                new
                {
                    searchString,
                    pageNumber,
                    pageSize,
                    sortOrder
                }
            );
        }
        TempData["Success"] = "Status Changed Successfully";

        return RedirectToAction(
            "Index",
            new
            {
                searchString,
                pageNumber,
                pageSize,
                sortOrder
            }
        );
    }

    /// <summary>
    /// Fetches Asset detail based on id
    /// </summary>

    private async Task<ResponseDto<AssetViewModel>> GetDetails(string id)
    {
        var client = _clientFactory.CreateClient("AssetManagementApi");
        var response = await client.GetAsync($"asset/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return new ResponseDto<AssetViewModel>
            {
                IsSuccess = false,
                Message = "Unable to fetch Asset Details"
            };
        }

        var result = await response.Content.ReadFromJsonAsync<ResponseDto<AssetViewModel>>();
        if (result == null || !result.IsSuccess || result.Data == null)
        {
            return new ResponseDto<AssetViewModel>
            {
                IsSuccess = false,
                Message = result?.Message ?? "Unable to fetcch User Details."
            };
        }

        return result;
    }

    /// <summary>
    ///return Asset Details view
    /// </summary>
    public async Task<IActionResult> Detail(string id)
    {
        var userDetail = await GetDetails(id);
        if (!userDetail.IsSuccess)
        {
            TempData["Error"] = userDetail.Message;
            return RedirectToAction(nameof(Index));
        }

        return View(userDetail.Data);
    }

    /// <summary>
    ///return confirmation view
    /// </summary>
    public async Task<IActionResult> Delete(string id)
    {
        var userDetail = await GetDetails(id);
        if (!userDetail.IsSuccess)
        {
            TempData["Error"] = userDetail.Message;
            return RedirectToAction(nameof(Index));
        }

        return View(userDetail.Data);
    }

    /// <summary>
    ///deletes a user
    /// </summary>


    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var client = _clientFactory.CreateClient("AssetManagementApi");

        var response = await client.DeleteAsync($"asset/{id}");
        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Unable to delete Asset";
            return RedirectToAction(nameof(Index));
        }
        var result = await response.Content.ReadFromJsonAsync<ResponseDto>();
        if (result == null || !result.IsSuccess)
        {
            TempData["Error"] = result?.Message ?? "Unable to delete user";
            return RedirectToAction(nameof(Index));
        }
        TempData["Success"] = result?.Message ?? "User Deleted Successfully";

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// return Asset detail view to update
    /// </summary>
    public async Task<IActionResult> Update(string id)
    {
        var response = await GetDetails(id);
        if (!response.IsSuccess || response.Data == null)
        {
            TempData["Error"] = response.Message;
            return RedirectToAction(nameof(Index));
        }
        PopulateDropdowns();

        return View(response.Data);
    }

    /// <summary>
    /// updates the user detail
    /// </summary>

    [HttpPost, ActionName("Update")]
    public async Task<IActionResult> UpdateConfirm(AssetViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Error = "Please provide valid data";
            return View();
        }
        var client = _clientFactory.CreateClient("AssetManagementApi");

        var assetDto = new UpdateAssetModel
        {
            SerialNumber = model.SerialNumber,
            CategoryId = model.CategoryId,
            DepartmentId = model.DepartmentId,
            Name = model.Name,
            Description = model.Description,
            ReceivedDate = model.ReceivedDate,
            IsActive = model.IsActive,
        };

        var jsonContent = System.Text.Json.JsonSerializer.Serialize(assetDto);
        var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
        var response = await client.PutAsync($"asset/{model.Id}", content);

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Unable to update asset";
            return RedirectToAction(nameof(Index));
        }
        var result = await response.Content.ReadFromJsonAsync<ResponseDto>();
        if (result == null || !result.IsSuccess)
        {
            TempData["Error"] = result?.Message ?? "Unable to update asset";
            return RedirectToAction(nameof(Index));
        }
        TempData["Success"] = result?.Message ?? "Asset Updated Successfully";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// returns asset with tag deta
    /// </summary>
    public async Task<IActionResult> ManageTag(string id)
    {
        var result = await GetDetails(id);
        if (!result.IsSuccess || result.Data == null)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        if (!string.IsNullOrEmpty(result.Data.TagMacAddress))
        {
            return View(result.Data);
        }

        var client = _clientFactory.CreateClient("AssetManagementApi");

        var response = await client.GetAsync("Tag/Available");
        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Error Fetching available Tags";
            return RedirectToAction("Index");
        }

        var tagResult = await response.Content.ReadFromJsonAsync<ResponseDto<List<TagViewModel>>>();

        if (tagResult == null || !tagResult.IsSuccess)
        {
            TempData["Error"] = "Error Fetching available Tags";
            return RedirectToAction("Index");
        }

        if (tagResult?.Data?.Count < 1)
        {
            TempData["Error"] = "No available tags to assign, First create some tags";
            return RedirectToAction("Index");
        }

        ViewBag.Tags = new SelectList(tagResult?.Data, "Id", "MacAddress");

        return View(result.Data);
    }

    /// <summary>
    /// Assiigns a tag to an asset
    /// </summary>


    [HttpPost]
    public async Task<IActionResult> AssignTag(string id, string selectedTagId)
    {
        if (string.IsNullOrEmpty(selectedTagId))
        {
            TempData["Error"] = "Please select a tag before submitting.";
            return RedirectToAction(nameof(ManageTag), new { id });
        }

        var client = _clientFactory.CreateClient("AssetManagementApi");

        var assignData = new { tagId = selectedTagId, assetId = id };
        var jsonContent = System.Text.Json.JsonSerializer.Serialize(assignData);
        var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
        var response = await client.PostAsync("Asset/Assign-Tag", content);

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Failed to assign tag. Please try again.";
            return RedirectToAction(nameof(ManageTag), new { id });
        }

        var result = await response.Content.ReadFromJsonAsync<ResponseDto>();
        if (result == null || !result.IsSuccess)
        {
            TempData["Error"] = result?.Message ?? "Error occurred while assigning tag.";
            return RedirectToAction(nameof(ManageTag), new { id });
        }

        TempData["Success"] = "Tag assigned successfully.";
        return RedirectToAction("Index");
    }

    /// <summary>
    /// removes tag from a asset
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> RemoveTag(string id)
    {
        var client = _clientFactory.CreateClient("AssetManagementApi");

        var response = await client.PostAsync($"Asset/UnAssign-Tag/{id}", null);

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Failed to remove tag. Please try again.";
            return RedirectToAction(nameof(ManageTag), new { id });
        }

        var result = await response.Content.ReadFromJsonAsync<ResponseDto>();
        if (result == null || !result.IsSuccess)
        {
            TempData["Error"] = result?.Message ?? "Error occurred while removing tag.";
            return RedirectToAction(nameof(ManageTag), new { id });
        }

        TempData["Success"] = "Tag removed successfully.";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Exports excelsheet containing all Asset details
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ExportToExcel()
    {
        var client = _clientFactory.CreateClient("AssetManagementApi");
        var response = await client.GetAsync("asset/export");
        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Failed to export Excel file.";
            return RedirectToAction("Index");
        }
        var fileBytes = await response.Content.ReadAsByteArrayAsync();

        var fileName =
            response.Content.Headers.ContentDisposition?.FileName?.Trim('"') ?? "Assets.xlsx";

        return File(
            fileBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileName
        );
    }
}
