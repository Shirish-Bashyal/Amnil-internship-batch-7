using AssetManagement.Contract.Service;
using AssetManagement.Domain.Dtos;
using AssetManagement.Domain.Dtos.User;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserServices _userService;

        public UsersController(IUserServices userService)
        {
            _userService = userService;
        }

        [HttpPost("create")]
        public async Task<ActionResult<ServiceResponseDto<Guid>>> CreateUser([FromBody] CreateUserDto dto)
        {
            var result = await _userService.CreateAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<ServiceResponseDto<bool>>> DeleteUser(Guid id)
        {
            var result = await _userService.DeleteAsync(id);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
        [HttpGet("all")]
        public async Task<ActionResult<ServiceResponseDto<IEnumerable<GetUserDto>>>> GetAllUsers()
        {
            var result = await _userService.GetAllAsync();
            return Ok(result);
        }
    }
}