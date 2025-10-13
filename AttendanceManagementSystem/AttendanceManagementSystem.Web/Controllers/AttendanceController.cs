using AttendanceManagementSystem.Shared.Dtos;
using AttendanceManagementSystem.Shared.Dtos.Attendance;
using AttendanceManagementSystem.Shared.Dtos.User;
using AttendanceManagementSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagementSystem.Web.Controllers;

public class AttendanceController : Controller
{
    private readonly IHttpClientFactory _clientFactory;

    public AttendanceController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    ///Add attandance for given user based on Id.
    /// </summary>

    [HttpPost]
    public async Task<IActionResult> CheckIn(string userId)
    {
        var client = _clientFactory.CreateClient("AttendanceApi");
        var response = await client.PostAsync($"Attendance/check-In/{userId}", null);

        if (response.IsSuccessStatusCode)
            TempData["Success"] = "Checked in successfully!";
        else
            TempData["Error"] = "Failed to check in.";

        return RedirectToAction("Index");
    }

    /// <summary>
    ///Checksout the user based on Id
    /// </summary>


    [HttpPost]
    public async Task<IActionResult> CheckOut(string userId)
    {
        var client = _clientFactory.CreateClient("AttendanceApi");
        var response = await client.PatchAsync($"Attendance/check-out/{userId}", null);

        if (response.IsSuccessStatusCode)
            TempData["Success"] = "Checked out successfully!";
        else
            TempData["Error"] = "Failed to check out.";

        return RedirectToAction("Index");
    }

    /// <summary>
    /// Fetches user detail based on id
    /// </summary>


    private async Task<ServiceResponseDto<GetUserDto>> UserDetails(string id)
    {
        var client = _clientFactory.CreateClient("AttendanceApi");
        var response = await client.GetAsync($"User/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return new ServiceResponseDto<GetUserDto>
            {
                IsSuccess = false,
                Message = "Unable to fetcch User Details."
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
    ///Fetches the attendance report for user based on Id
    /// </summary>


    [HttpPost]
    public async Task<IActionResult> Report(string userId)
    {
        var userDetail = await UserDetails(userId);
        if (!userDetail.IsSuccess)
        {
            TempData["Error"] = userDetail.Message;
            return View("Index");
        }

        var client = _clientFactory.CreateClient("AttendanceApi");
        var response = await client.GetAsync($"Attendance/{userId}");

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Failed to load attendance report.";
            return View("Index");
        }

        var result = await response.Content.ReadFromJsonAsync<
            ServiceResponseDto<IEnumerable<AttendanceDto>>
        >();
        if (result == null || !result.IsSuccess || result.Data == null)
        {
            TempData["Error"] = result?.Message ?? "Failed to load attendance report";
            return View("Index");
        }

        var report = new ReportViewModel
        {
            Attendance = result?.Data ?? new List<AttendanceDto>(),
            Name = userDetail.Data!.Name,
            PhoneNumber = userDetail.Data.PhoneNumber,
        };

        return View(report);
    }
}
