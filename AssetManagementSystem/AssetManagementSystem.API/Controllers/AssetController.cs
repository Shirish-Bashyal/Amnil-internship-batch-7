using AssetManagementSystem.Contracts.Assets;
using AssetManagementSystem.Shared.Constants;
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
    public async Task<ActionResult<ResponseDto>> Create([FromForm] CreateAssetDto asset)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdAsset = await _assetService.CreateAsync(asset);

        return StatusCode(createdAsset.StatusCode, createdAsset);
    }

    /// <summary>
    /// Updates an existing asset.
    /// </summary>
    /// <param name="id">The Id of the asset to update</param>
    /// <param name="asset">The details of the asset to update</param>
    /// <returns>Return success status</returns>
    [HttpPut("{id}")]
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
    public async Task<ActionResult<ResponseDto>> GetList(
        [FromQuery] PagedFilterRequestDto filter,
        [FromQuery] AssetFilter input
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _assetService.GetListAsync(filter, input);

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
    public async Task<IActionResult> Export(string format)
    {
        var (fileStream, fileName, contentType) = await _assetService.ExportAsync(format);

        if (fileStream == null)
        {
            return NotFound(new ResponseDto { Message = "No assets found to export." });
        }

        // Set headers for file download
        return File(fileStream, contentType, fileName);
    }

    /// <summary>
    ///changes the status of asset as per input
    /// </summary>


    [HttpPatch("{id}/{isActive}")]
    public async Task<ActionResult<ResponseDto>> ChangeStatus(Guid id, bool isActive)
    {
        var result = await _assetService.ChangeStatusAsync(id, isActive);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Assigns a tag to an entity
    /// </summary>
    [HttpPost("Assign-Tag")]
    public async Task<ActionResult<ResponseDto>> AssignTag(AssignTagDto input)
    {
        var result = await _assetService.AssignTagAsync(input);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// removes tag from an entity
    /// </summary>
    [HttpPost("UnAssign-Tag/{id}")]
    public async Task<ActionResult<ResponseDto>> UnAssignTag(Guid id)
    {
        var result = await _assetService.UnAssignTagAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Assigns a Asset to an User
    /// </summary>
    [HttpPost("Assign-User")]
    public async Task<ActionResult<ResponseDto>> AssignUser(AssignUserDto input)
    {
        var result = await _assetService.AssignUserAsync(input);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// removes Asset from an User
    /// </summary>
    [HttpPost("UnAssign-User/{id}")]
    public async Task<ActionResult<ResponseDto>> UnAssignUser(Guid id)
    {
        var result = await _assetService.UnAssignUserAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("users")]
    public async Task<ActionResult<ResponseDto>> GetUsers()
    {
        var result = await _assetService.GetUserAsync();
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("buildings")]
    public async Task<ActionResult<ResponseDto>> GetBuildings()
    {
        var result = await _assetService.GetBuildingAsync();
        return StatusCode(result.StatusCode, result);
    }
}
