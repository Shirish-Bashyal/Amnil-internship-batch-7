using AssetManagementSystem.Contract.Interfaces.Service;
using AssetManagementSystem.Shared;
using AssetManagementSystem.Shared.Dtos;
using AssetManagementSystem.Shared.Dtos.Asset;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagementSystem.API.Controllers;

[Route("api/[controller]")]
public class AssetController : Controller
{
    private readonly IAssetService _assetService;
    public AssetController(IAssetService assetService)
    {
        _assetService = assetService;
    }


    [HttpPost]

    public async Task<ActionResult<ResponseData<Guid>>> Create([FromBody] AssetDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        ResponseData<Guid> result = await _assetService.CreateAsync(dto);

        if (result.IsSuccess)
        {
            return StatusCode(StatusCodes.Status201Created, result); // 201 Created
        }


        return NotFound(result);
    }

    [HttpGet]

    public async Task<ActionResult<ResponseData<IEnumerable<AssetReadDto>>>> GetAllAsync()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _assetService.GetAllAsync();
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return NotFound(result);
    }



    [HttpGet("{id}")]

    public async Task<ActionResult<ResponseData<AssetReadDto>>> GetAsync(Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _assetService.GetAsync(id);
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return NotFound(result);
    }


    [HttpPut("{id}")]
    public async Task<ActionResult<ResponseData<bool>>> UpdateAsync(Guid id, [FromBody] AssetDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _assetService.UpdateAsync(id, dto);
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

        var result = await _assetService.DeleteAsync(id);

        if (result.IsSuccess)
            return Ok(result);

        return NotFound(result);
    }

    [HttpPatch("{id}/activation")]
    public async Task<ActionResult<ResponseData<bool>>> ChangeActivationStatus(Guid id, [FromBody] bool isActivated)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _assetService.ChangeActivationStatusAsync(id, isActivated);

        if (result.IsSuccess)
            return Ok(result);

        return NotFound(result);
    }


    [HttpGet("List")]
    public async Task<ActionResult<ResponseData<PagedResponseDto<AssetDto>>>> GetList([FromQuery] PaginationAndFilterRequest filter)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _assetService.GetListAsync(filter);

        if (result.IsSuccess)
            return Ok(result);

        return NotFound(result);
    }
}
