using AssetManagementSystem.Contracts.Tags;
using AssetManagementSystem.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagementSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TagController : ControllerBase
{
    private readonly ITagService _tagService;

    public TagController(ITagService tagService)
    {
        _tagService = tagService;
    }

    /// <summary>
    /// creates new Tag
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ResponseDto>> Create([FromBody] CreateTagDto tag)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var createdTag = await _tagService.CreateAsync(tag);

        return StatusCode(createdTag.StatusCode, createdTag);
    }

    /// <summary>
    /// updates an existing Tag.
    /// </summary>
    /// <param name="id"> The Id of the tag to update</param>
    /// <param name="Tag">The details of the tag to update </param>
    /// <returns> return success status </returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<ResponseDto>> Update(Guid id, [FromBody] UpdateTagDto tag)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _tagService.UpdateAsync(id, tag);

        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Retrives Tags with pagination
    /// </summary>
    /// <returns> a list of Tags</returns>
    [HttpGet("List")]
    public async Task<ActionResult<ResponseDto>> GetList([FromQuery] PagedFilterRequestDto filter)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _tagService.GetListAsync(filter);

        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Retrives a Tag
    /// </summary>
    /// <param name="id"> The Id of the Tag to retrive</param>
    /// <returns>  Tag details</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ResponseDto>> Get(Guid id)
    {
        var result = await _tagService.GetAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Deletes a Tag by its Id
    /// </summary>
    /// <param name="id"> The Id of the Tag to delete </param>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ResponseDto>> Delete(Guid id)
    {
        var result = await _tagService.DeleteAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// returns data in excel format
    /// </summary>
    [HttpGet("export")]
    public async Task<IActionResult> Export()
    {
        var (fileStream, fileName) = await _tagService.ExportToExcelAsync();

        if (fileStream == null)
        {
            return NotFound(new ResponseDto { Message = "No Tags found to export." });
        }

        // Set headers for file download
        return File(
            fileStream,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileName
        );
    }

    /// <summary>
    ///gets all active and unassigned tags
    /// </summary>
    /// <returns>a list of active and unassigned tags</returns>

    [HttpGet("Available")]
    public async Task<ActionResult<List<ResponseDto>>> GetAvailableList()
    {
        var result = await _tagService.GetAvailableListAsync();
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    ///changes status of tag entity
    /// </summary>

    [HttpPatch("{id}/{isActive}")]
    public async Task<ActionResult<ResponseDto>> ChangeStatus(Guid id, bool isActive)
    {
        var result = await _tagService.ChangeStatusAsync(id, isActive);
        return StatusCode(result.StatusCode, result);
    }
}
