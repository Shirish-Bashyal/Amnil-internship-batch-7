using AssetManagementSystem.Contract.Interfaces.Service;
using AssetManagementSystem.Entity.Entities;
using AssetManagementSystem.Infrastructure.Data;
using AssetManagementSystem.Shared.Dtos.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;

namespace AssetManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _service;
       
        public AuthController(IAuthService service)
        {
            _service = service;
            
        }


        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto user)
        {
            var response = await _service.Register(user);

            if (response.IsSuccess == false) { return BadRequest(response.Message); }

            return Ok(response.Data);
        }


        [HttpPost("login")]

        public async Task<ActionResult<TokenRefreshDto>> Login(UserLoginDto user)
        {
            var response = await _service.Login(user);

            if (response == null) { return BadRequest(" Password nOt matched"); }

            if (response.isSuccess == false) { return BadRequest(response.message); }

            return Ok(new TokenRefreshDto
            {
                Token = response.Token,
                RefereshToken = response.RefereshToken

            });
        }


        [HttpPost("Refresh-token")]
        [Authorize]
        public async Task<ActionResult<TokenRefreshDto>> RefereshTokenGenerate(RefereshTokenRequestDto request)
        {
            var response = await _service.RefereshTokenGenerate(request);

            return Ok(new TokenRefreshDto
            {
                Token = response.Token,
                RefereshToken = response.RefereshToken

            });
        }

        [HttpGet("Admin-access")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AdminCheck()
        {

            return Ok("Admin login");
        }
    }
}
