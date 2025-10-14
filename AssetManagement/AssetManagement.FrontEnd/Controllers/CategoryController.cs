using AssetManagement.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace AssetManagement.Web.Controllers;

public class CategoryController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CategoryController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // GET: /Category
    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("AssetApi");
        var response = await client.GetAsync("api/Category");

        if (!response.IsSuccessStatusCode)
            return View("Error");

        var json = await response.Content.ReadAsStringAsync();
        var categories = JsonSerializer.Deserialize<List<CategoryDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return View(categories);
    }

    // GET: /Category/Create
    public IActionResult Create() => View();

    // POST: /Category/Create
    [HttpPost]
    public async Task<IActionResult> Create(CategoryDto categoryDto)
    {
        var client = _httpClientFactory.CreateClient("AssetApi");
        var content = new StringContent(JsonSerializer.Serialize(categoryDto), Encoding.UTF8, "application/json");

        var response = await client.PostAsync("api/Category", content);

        if (!response.IsSuccessStatusCode)
            return View("Error");

        return RedirectToAction("Index");
    }
}