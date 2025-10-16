using AssetManagement.Contract.Service;
using AssetManagement.Domain.Dtos;
using AssetManagement.Domain.Dtos.AssetDto;
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

        [HttpPost]
        public async Task<IActionResult> CreateAsset([FromBody] CreateAssetDto assetDto)
        {
            var result = await _assetService.CreateAsync(assetDto);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAssets()
        {
            var result = await _assetService.GetAllAsync();
            return Ok(result);
        }
        [HttpPost("paged")]
        public async Task<ActionResult<PageResponse<GetAssetDto>>> GetPagedAssets([FromBody] PageRequest request)
        {
            var result = await _assetService.GetFilteredContent(request);
            return Ok(result);
        }
        [HttpPut("update")]
        public async Task<ActionResult<ServiceResponseDto<bool>>> UpdateAsset([FromBody] UpdateAssetDto assetDto)
        {
            var result = await _assetService.UpdateAsync(assetDto);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<ServiceResponseDto>> DeleteAsset(Guid id)
        {
            var result = await _assetService.DeleteAsync(id);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

    }
}
