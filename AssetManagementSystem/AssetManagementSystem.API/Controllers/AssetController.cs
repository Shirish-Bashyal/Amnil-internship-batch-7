using AssetManagementSystem.Contracts.Assets;
using AssetManagementSystem.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagementSystem.API.Controllers;

/// <summary>
///Asset Api controller exposing endpoints
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AssetController : ControllerBase
{
    private readonly IAssetService _assetService;

    public AssetController(IAssetService assetService)
    {
        _assetService = assetService;
    }

    /// <summary>
    /// creates new Asset
    /// </summary>

    [HttpPost]
    public async Task<ActionResult<ResponseDto>> Create([FromBody] CreateAssetDto asset)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var createdAsset = await _assetService.CreateAsync(asset);

        return StatusCode(createdAsset.StatusCode, createdAsset);
    }

    /// <summary>
    /// updates an existing asset.
    /// </summary>
    /// <param name="id"> The Id of the aset to update</param>
    /// <param name="asset">The details of the aset to update </param>
    /// <returns> return success status </returns>
    [HttpPut("id")]
    public async Task<ActionResult<ResponseDto>> Update(Guid id, [FromBody] UpdateAssetDto asset)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _assetService.UpdateAsync(id, asset);

        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Retrives Assets with pagination
    /// </summary>
    /// <returns> a list of Assets</returns>

    [HttpGet("List")]
    public async Task<ActionResult<ResponseDto>> GetList([FromQuery] PagedFilterRequestDto filter)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _assetService.GetListAsync(filter);

        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Retrives a Asset
    /// </summary>
    /// <param name="id"> The Id of the Asset to retrive</param>
    /// <returns>  Asset details</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ResponseDto>> Get(Guid id)
    {
        var result = await _assetService.GetAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Deletes a Asset by its Id
    /// </summary>
    /// <param name="id"> The Id of the Asset to delete </param>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ResponseDto>> Delete(Guid id)
    {
        var result = await _assetService.DeleteAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// returns data in excel format
    /// </summary>


    [HttpGet("export")]
    public async Task<IActionResult> Export()
    {
        var (fileStream, fileName) = await _assetService.ExportToExcelAsync();

        if (fileStream == null)
        {
            return NotFound(new ResponseDto { Message = "No assets found to export." });
        }

        // Set headers for file download
        return File(
            fileStream,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileName
        );
    }
}
