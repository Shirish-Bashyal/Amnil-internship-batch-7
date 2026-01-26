using System.Diagnostics;
using AssetManagementSystem.Client.Models;
using AssetManagementSystem.Client.Models.Assets;
using AssetManagementSystem.Client.Models.Dashboard;
using AssetManagementSystem.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagementSystem.Client.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHttpClientFactory _clientFactory;

    public HomeController(ILogger<HomeController> logger, IHttpClientFactory client)
    {
        _logger = logger;
        _clientFactory = client;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var client = _clientFactory.CreateClient("AssetManagementApi");

        var response = await client.GetAsync("dashboard");
        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "Unable to fetch.";
            return View();
        }
        var result = await response.Content.ReadFromJsonAsync<ResponseDto<DashboardViewModel>>();

        if (result == null || !result.IsSuccess || result.Data == null)
        {
            ViewBag.Error = result?.Message;
            return View();
        }

        return View(result.Data);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        );
    }
}
