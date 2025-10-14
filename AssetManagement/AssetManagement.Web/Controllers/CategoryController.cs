using AssetManagement.Contract.Service;
using AssetManagement.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // POST: api/Category
    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryDto categoryDto)
    {
        var result = await _categoryService.CreateAsync(categoryDto);
        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    // GET: api/Category
    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var result = await _categoryService.GetAllAsync();
        return Ok(result);
    }
}
