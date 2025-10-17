using AssetManagementSystem.Application.Services;
using AssetManagementSystem.Contract.Interfaces.Service;
using AssetManagementSystem.Shared.Dtos;
using AssetManagementSystem.Shared.Dtos.Department;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagementSystem.API.Controllers;

[Route("api/[controller]")]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _departmentService;
    public DepartmentController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }


    [HttpPost]

    public async Task<ActionResult<ResponseData<Guid>>> Create([FromBody] DepartmentDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        ResponseData<Guid> result = await _departmentService.CreateAsync(dto);

        if (result.IsSuccess)
        {
            return StatusCode(StatusCodes.Status201Created, result); // 201 Created
        }


        return NotFound(result);
    }

    [HttpGet]

    public async Task<ActionResult<ResponseData<IEnumerable<DepartmentReadDto>>>> GetAllAsync()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _departmentService.GetAllAsync();
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return NotFound(result);
    }



    [HttpGet("{id}")]

    public async Task<ActionResult<ResponseData<DepartmentReadDto>>> GetAsync(Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _departmentService.GetAsync(id);
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return NotFound(result);
    }


    [HttpPut("{id}")]
    public async Task<ActionResult<ResponseData<bool>>> UpdateAsync(Guid id, [FromBody] DepartmentDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _departmentService.UpdateAsync(id, dto);
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

        var result = await _departmentService.DeleteAsync(id);

        if (result.IsSuccess)
            return Ok(result);

        return NotFound(result);
    }

}
