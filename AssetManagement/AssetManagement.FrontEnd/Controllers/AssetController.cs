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
    //public async Task<IActionResult> Index()
    //{
    //    var client = _httpClientFactory.CreateClient("AssetApi");
    //    var response = await client.GetAsync("api/Asset");

    //    if (!response.IsSuccessStatusCode)
    //        return View("Error");

    //    //var json = await response.Content.ReadAsJsonAsync();
    //    //var assets = JsonSerializer.Deserialize<List<AssetDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    //    var json = await response.Content.ReadFromJsonAsync<
    //        ServiceResponseDto<IEnumerable<AssetDto>>
    //    >();
    //    var assets = json.Data?.ToList() ?? new List<AssetDto>();
    //    return View(assets); 
    //}
    public async Task<IActionResult> Index(int page = 1, int pageSize = 15)
    {
        var client = _httpClientFactory.CreateClient("AssetApi");

        var request = new PageRequest
        {
            SkipPageCount = page,
            ListCount = pageSize
        };

        var response = await client.PostAsJsonAsync("api/Asset/paged", request);

        if (!response.IsSuccessStatusCode)
            return View("Error");
        var result = await response.Content.ReadFromJsonAsync<PageResponse<GetAssetDto>>();

        if (result == null || result.Items == null)
            return View("Error");

        return View(result);

    }

     
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(CreateAssetDto assetDto)
    {
        var client = _httpClientFactory.CreateClient("AssetApi");
        var content = new StringContent(JsonSerializer.Serialize(assetDto), Encoding.UTF8, "application/json");

        var response = await client.PostAsync("api/Asset", content);

        if (!response.IsSuccessStatusCode)
            return View("Error");

        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult Update()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateAssetDto assetDto)
    {
        var client = _httpClientFactory.CreateClient("AssetApi");
        if (assetDto.Id == Guid.Empty)
        {
            ViewBag.Error = $"Invalid asset ID: {assetDto.Id} (Guid.Empty)";
            return View(assetDto);
        }
        var content = new StringContent(JsonSerializer.Serialize(assetDto), Encoding.UTF8, "application/json");
        var response = await client.PutAsync("api/Asset/update", content);

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "Failed to update asset.";
            return View(assetDto);
        }

        return RedirectToAction("Index");
    }

}