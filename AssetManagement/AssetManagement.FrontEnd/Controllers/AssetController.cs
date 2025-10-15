using AssetManagement.Domain.Dtos;
using AssetManagement.Domain.Dtos.AssetDto;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace AssetManagement.Web.Controllers;

public class AssetController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AssetController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("AssetApi");
        var response = await client.GetAsync("api/Asset");

        if (!response.IsSuccessStatusCode)
            return View("Error");

        //var json = await response.Content.ReadAsJsonAsync();
        //var assets = JsonSerializer.Deserialize<List<AssetDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        var json = await response.Content.ReadFromJsonAsync<
            ServiceResponseDto<IEnumerable<AssetDto>>
        >();
        var assets = json.Data?.ToList() ?? new List<AssetDto>();
        return View(assets); 
    }

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(AssetDto assetDto)
    {
        var client = _httpClientFactory.CreateClient("AssetApi");
        var content = new StringContent(JsonSerializer.Serialize(assetDto), Encoding.UTF8, "application/json");

        var response = await client.PostAsync("api/Asset", content);

        if (!response.IsSuccessStatusCode)
            return View("Error");

        return RedirectToAction("Index");
    }
}