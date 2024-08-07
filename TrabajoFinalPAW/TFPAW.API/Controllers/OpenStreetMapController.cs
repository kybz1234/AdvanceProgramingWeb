using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TFPAW.Service;

namespace TFPAW.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpenStreetMapController : ControllerBase
    {

        private readonly OpenStreetMapService _osmService;

        public OpenStreetMapController(OpenStreetMapService osmService)
        {
            _osmService = osmService;
        }

        [HttpGet("mapdata")]
        public async Task<IActionResult> GetMapData(double minLon, double minLat, double maxLon, double maxLat)
        {
            var data = await _osmService.GetMapDataByBoundingBoxAsync(minLon, minLat, maxLon, maxLat);
            return Ok(data);
        }

        [HttpGet("nodedata")]
        public async Task<IActionResult> GetNodeData(long nodeId)
        {
            var data = await _osmService.GetNodeDataAsync(nodeId);
            return Ok(data);
        }

    }
}
