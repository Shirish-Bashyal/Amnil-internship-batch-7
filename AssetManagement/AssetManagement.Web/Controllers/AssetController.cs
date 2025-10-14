using AssetManagement.Contract.Service;
using AssetManagement.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssetController : ControllerBase
    {
        private readonly IAssetService _assetService;

        public AssetController(IAssetService assetService)
        {
            _assetService = assetService;
        }

        // POST: api/Asset
        [HttpPost]
        public async Task<IActionResult> CreateAsset([FromBody] AssetDto assetDto)
        {
            var result = await _assetService.CreateAsync(assetDto);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // GET: api/Asset
        [HttpGet]
        public async Task<IActionResult> GetAllAssets()
        {
            var result = await _assetService.GetAllAsync();
            return Ok(result);
        }
    } 
}
