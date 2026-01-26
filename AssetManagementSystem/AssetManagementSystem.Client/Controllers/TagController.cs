using AssetManagementSystem.Client.Models;
using AssetManagementSystem.Client.Models.Assets;
using AssetManagementSystem.Client.Models.Tag;
using AssetManagementSystem.Shared.Dtos;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagementSystem.Client.Controllers;

public class TagController : Controller
{
    private readonly IHttpClientFactory _clientFactory;

    public TagController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Create()
    {
        return View();
    }

    /// <summary>
    /// adds a new Tag
    /// </summary>

    [HttpPost]
    public async Task<IActionResult> Create(TagModel tag)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Error = "Please provide valid data";

            return View();
        }

        var client = _clientFactory.CreateClient("AssetManagementApi");
        var jsonContent = System.Text.Json.JsonSerializer.Serialize(tag);
        var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
        var response = await client.PostAsync("tag", content);

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "Tag Creation Failed";

            return View(tag);
        }
        TempData["Success"] = "Tag created successfully";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    ///fetches Tag based on given filters
    /// </summary>

    [HttpGet]
    public async Task<IActionResult> Index(
        string searchString,
        int pageNumber,
        int pageSize,
        string sortOrder,
        bool? status
    )
    {
        ViewData["CurrentFilter"] = searchString;

        if (string.IsNullOrEmpty(sortOrder))
        {
            sortOrder = "name_desc";
            TempData["NameSortParam"] = sortOrder;
        }
        else
        {
            TempData["NameSortParam"] = sortOrder == "name_desc" ? "name_asc" : "name_desc";
        }
        TempData["CurrentSort"] = sortOrder;
        TempData["Status"] = status;

        if (pageSize < 5)
            pageSize = 5;

        if (pageNumber < 1)
            pageNumber = 1;
        int skipCount = (pageNumber - 1) * pageSize;

        var client = _clientFactory.CreateClient("AssetManagementApi");

        var response = await client.GetAsync(
            $"tag/list?SearchTerm={searchString}&SkipCount={skipCount}&MaxResultCount={pageSize}&SortOrder={sortOrder}&IsActive={status}"
        );
        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "Unable to fetch Tags.";
            return View();
        }
        var result = await response.Content.ReadFromJsonAsync<
            ResponseDto<PagedResponseDto<TagViewModel>>
        >();

        if (result == null || !result.IsSuccess || result.Data == null)
        {
            ViewBag.Error = result?.Message;
            return View(new List<TagViewModel>());
        }
        //viewmodel
        var PagedResult = new PagedViewModel<TagViewModel>
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
    /// Returns Tag detail view for update
    /// </summary>
    public async Task<IActionResult> Update(string id)
    {
        var client = _clientFactory.CreateClient("AssetManagementApi");
        var response = await client.GetAsync($"tag/{id}");

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Unable to fetch tag details.";
            return RedirectToAction(nameof(Index));
        }

        var result = await response.Content.ReadFromJsonAsync<ResponseDto<TagViewModel>>();
        if (result == null || !result.IsSuccess || result.Data == null)
        {
            TempData["Error"] = result?.Message ?? "Tag details not found.";
            return RedirectToAction(nameof(Index));
        }

        return View(result.Data);
    }

    /// <summary>
    /// Updates the Tag details
    /// </summary>
    [HttpPost, ActionName("Update")]
    public async Task<IActionResult> UpdateConfirm(TagViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Error = "Please provide valid data";
            return View(model);
        }

        var client = _clientFactory.CreateClient("AssetManagementApi");

        var updateTagDto = new UpdateTagModel
        {
            MacAddress = model.MacAddress,
            IsActive = model.IsActive
        };

        var jsonContent = System.Text.Json.JsonSerializer.Serialize(updateTagDto);
        var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

        var response = await client.PutAsync($"tag/{model.Id}", content);

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Unable to update tag.";
            return RedirectToAction(nameof(Index));
        }

        var result = await response.Content.ReadFromJsonAsync<ResponseDto>();
        if (result == null || !result.IsSuccess)
        {
            TempData["Error"] = result?.Message ?? "Unable to update tag.";
            return RedirectToAction(nameof(Index));
        }

        TempData["Success"] = result?.Message ?? "Tag updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Returns confirmation view for deleting a Tag
    /// </summary>
    public async Task<IActionResult> Delete(string id)
    {
        var client = _clientFactory.CreateClient("AssetManagementApi");
        var response = await client.GetAsync($"tag/{id}");

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Unable to fetch tag details.";
            return RedirectToAction(nameof(Index));
        }

        var result = await response.Content.ReadFromJsonAsync<ResponseDto<TagViewModel>>();
        if (result == null || !result.IsSuccess || result.Data == null)
        {
            TempData["Error"] = result?.Message ?? "Tag details not found.";
            return RedirectToAction(nameof(Index));
        }

        return View(result.Data);
    }

    /// <summary>
    /// Deletes a Tag
    /// </summary>
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var client = _clientFactory.CreateClient("AssetManagementApi");

        var response = await client.DeleteAsync($"tag/{id}");
        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Unable to delete Tag";
            return RedirectToAction(nameof(Index));
        }

        var result = await response.Content.ReadFromJsonAsync<ResponseDto>();
        if (result == null || !result.IsSuccess)
        {
            TempData["Error"] = result?.Message ?? "Unable to delete Tag";
            return RedirectToAction(nameof(Index));
        }

        TempData["Success"] = result?.Message ?? "Tag deleted successfully";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Exports Excel sheet containing all Tag details
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ExportToExcel()
    {
        var client = _clientFactory.CreateClient("AssetManagementApi");
        var response = await client.GetAsync("tag/export");

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Failed to export Excel file.";
            return RedirectToAction("Index");
        }

        var fileBytes = await response.Content.ReadAsByteArrayAsync();

        var fileName =
            response.Content.Headers.ContentDisposition?.FileName?.Trim('"') ?? "Tags.xlsx";

        return File(
            fileBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileName
        );
    }

    /// <summary>
    /// changes status of tag entity
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> ToggleActive(
        Guid id,
        string newStatus,
        string? searchString,
        int pageNumber = 1,
        int pageSize = 5,
        string? sortOrder = null
    )
    {
        Console.WriteLine($"{newStatus}");

        var isActive = false;
        if (newStatus == "Active")
        {
            isActive = true;
        }

        var client = _clientFactory.CreateClient("AssetManagementApi");
        var response = await client.PatchAsync($"tag/{id}/{isActive}", null);
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
}
