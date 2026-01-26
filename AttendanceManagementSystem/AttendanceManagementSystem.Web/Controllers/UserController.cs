using AttendanceManagementSystem.Shared.Dtos;
using AttendanceManagementSystem.Shared.Dtos.User;
using AttendanceManagementSystem.Web.Models;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagementSystem.Web.Controllers;

public class UserController : Controller
{
    private readonly IHttpClientFactory _clientFactory;

    public UserController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    /// <summary>
    /// returns create view
    /// </summary>

    public IActionResult Create()
    {
        return View();
    }

    /// <summary>
    /// adds a new user
    /// </summary>

    [HttpPost]
    public async Task<IActionResult> Create(UserDto user)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Please provide valid data";
            return View();
        }

        var client = _clientFactory.CreateClient("AttendanceApi");
        var jsonContent = System.Text.Json.JsonSerializer.Serialize(user);
        var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
        var response = await client.PostAsync("user", content);
        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "User not created";
            return View(user);
        }
        TempData["Success"] = "User created successfully";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// exports excelsheet containing all user details
    /// </summary>

    [HttpGet]
    public async Task<IActionResult> ExportToExcel()
    {
        var client = _clientFactory.CreateClient("AttendanceApi");
        var response = await client.GetAsync("user");
        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Unable to get all users";
            RedirectToAction(nameof(Index));
        }

        var result = await response.Content.ReadFromJsonAsync<
            ServiceResponseDto<IEnumerable<GetUserDto>>
        >();
        if (result == null || !result.IsSuccess || result.Data == null)
        {
            TempData["Error"] = result?.Message;
            RedirectToAction(nameof(Index));
        }

        var users = result?.Data;

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Users");
        worksheet.Cell(1, 1).Value = "Id";

        worksheet.Cell(1, 2).Value = "Name";
        worksheet.Cell(1, 3).Value = "Phone Number";
        worksheet.Cell(1, 4).Value = "Created At";

        int row = 2;
        foreach (var user in users)
        {
            worksheet.Cell(row, 1).Value = user.Id.ToString();

            worksheet.Cell(row, 2).Value = user.Name;
            worksheet.Cell(row, 3).Value = user.PhoneNumber;
            worksheet.Cell(row, 4).Value = user.CreationAt;

            row++;
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        var content = stream.ToArray();

        return File(
            content,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "Users.xlsx"
        );
    }

    /// <summary>
    ///fetches user based on given filters
    /// </summary>

    [HttpGet]
    public async Task<IActionResult> Index(
        string searchString,
        int pageNumber,
        int pageSize,
        string sortOrder
    )
    {
        ViewData["CurrentFilter"] = searchString;

        if (string.IsNullOrEmpty(sortOrder))
        {
            sortOrder = "date_desc";
            TempData["CreatedAtSortParam"] = "date_desc";
        }
        else
        {
            TempData["CreatedAtSortParam"] = sortOrder == "date_desc" ? "date_asc" : "date_desc";

            TempData["NameSortParam"] = sortOrder == "name_desc" ? "name_asc" : "name_desc";
        }
        TempData["CurrentSort"] = sortOrder;

        if (pageSize < 5)
            pageSize = 5;

        if (pageNumber < 1)
            pageNumber = 1;
        int skipCount = (pageNumber - 1) * pageSize;

        var client = _clientFactory.CreateClient("AttendanceApi");

        var response = await client.GetAsync(
            $"user/list?SearchTerm={searchString}&SkipCount={skipCount}&MaxResultCount={pageSize}&SortOrder={sortOrder}"
        );
        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "Unable to fetch users.";
            return View(new List<GetUserDto>());
        }
        var result = await response.Content.ReadFromJsonAsync<
            ServiceResponseDto<PagedResponseDto<GetUserDto>>
        >();

        if (result == null || !result.IsSuccess || result.Data == null)
        {
            ViewBag.Error = result?.Message;
            return View(new List<GetUserDto>());
        }
        //viewmodel
        var PagedResult = new UserViewModel
        {
            User = result.Data.Items,
            TotalItems = result.Data.TotalCount,
            TotalPages = (int)Math.Ceiling((double)result.Data.TotalCount / pageSize),
            PageIndex = pageNumber,
            PageSize = pageSize
        };

        return View(PagedResult);
    }

    /// <summary>
    /// Fetches user detail based on id
    /// </summary>

    private async Task<ServiceResponseDto<GetUserDto>> Details(string id)
    {
        var client = _clientFactory.CreateClient("AttendanceApi");
        var response = await client.GetAsync($"User/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return new ServiceResponseDto<GetUserDto>
            {
                IsSuccess = false,
                Message = "Unable to fetcch User Details"
            };
        }

        var result = await response.Content.ReadFromJsonAsync<ServiceResponseDto<GetUserDto>>();
        if (result == null || !result.IsSuccess || result.Data == null)
        {
            return new ServiceResponseDto<GetUserDto>
            {
                IsSuccess = false,
                Message = result?.Message ?? "Unable to fetcch User Details."
            };
        }

        return result;
    }

    /// <summary>
    ///return confirmation view
    /// </summary>


    public async Task<IActionResult> Delete(string id)
    {
        var userDetail = await Details(id);
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
        var client = _clientFactory.CreateClient("AttendanceApi");

        var response = await client.DeleteAsync($"user/{id}");
        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Unable to delete user";
            return RedirectToAction(nameof(Index));
        }
        var result = await response.Content.ReadFromJsonAsync<ServiceResponseDto<bool>>();
        if (result == null || !result.IsSuccess)
        {
            TempData["Error"] = result?.Message ?? "Unable to delete user";
            return RedirectToAction(nameof(Index));
        }
        TempData["Success"] = result?.Message ?? "User Deleted Successfully";

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// return user detail view to update
    /// </summary>

    public async Task<IActionResult> Update(string id)
    {
        var response = await Details(id);
        if (!response.IsSuccess || response.Data == null)
        {
            TempData["Error"] = response.Message;
            return RedirectToAction(nameof(Index));
        }
        var userDetail = new UpdateUserDto
        {
            Name = response.Data.Name,
            PhoneNumber = response.Data.PhoneNumber,
            Id = response.Data.Id,
        };

        return View(userDetail);
    }

    /// <summary>
    /// updates the user detail
    /// </summary>

    [HttpPost, ActionName("Update")]
    public async Task<IActionResult> UpdateConfirm(UpdateUserDto model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Please provide valid data";
            return View();
        }
        var client = _clientFactory.CreateClient("AttendanceApi");
        var user = new UserDto { Name = model.Name, PhoneNumber = model.PhoneNumber, };
        var jsonContent = System.Text.Json.JsonSerializer.Serialize(user);
        var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
        var response = await client.PutAsync($"user/{model.Id}", content);

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Unable to update user";
            return RedirectToAction(nameof(Index));
        }
        var result = await response.Content.ReadFromJsonAsync<ServiceResponseDto<bool>>();
        if (result == null || !result.IsSuccess)
        {
            TempData["Error"] = result?.Message ?? "Unable to update user";
            return RedirectToAction(nameof(Index));
        }
        TempData["Success"] = result?.Message ?? "User Updated Successfully";
        return RedirectToAction(nameof(Index));
    }
}
