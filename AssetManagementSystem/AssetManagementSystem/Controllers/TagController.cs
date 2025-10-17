using AssetManagementSystem.Contract.Interfaces.Service;
using AssetManagementSystem.Shared.Dtos;
using AssetManagementSystem.Shared.Dtos.Department;
using AssetManagementSystem.Shared.Dtos.Tag;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagementSystem.API.Controllers;

[Route("api/[controller]")]
public class TagController : ControllerBase
{
    private readonly ITagService _tagService;
    public TagController (ITagService tagService)
    {
        _tagService = tagService;
    }


    [HttpPost]

    public async Task<ActionResult<ResponseData<Guid>>> Create([FromBody] TagDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        ResponseData<Guid> result = await _tagService.CreateAsync(dto);

        if (result.IsSuccess)
        {
            return StatusCode(StatusCodes.Status201Created, result); // 201 Created
        }


        return NotFound(result);
    }

    [HttpGet]

    public async Task<ActionResult<ResponseData<IEnumerable<TagReadDto>>>> GetAllAsync()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _tagService.GetAllAsync();
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return NotFound(result);
    }



    [HttpGet("{id}")]

    public async Task<ActionResult<ResponseData<TagReadDto>>> GetAsync(Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _tagService.GetAsync(id);
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return NotFound(result);
    }


    [HttpPut("{id}")]
    public async Task<ActionResult<ResponseData<bool>>> UpdateAsync(Guid id, [FromBody] TagDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _tagService.UpdateAsync(id, dto);
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return NotFound(result);

    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ResponseData<bool>>> DeleteAsync(Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _tagService.DeleteAsync(id);

        if (result.IsSuccess)
            return Ok(result);

        return NotFound(result);
    }
}
