using AgentsRest.Dto;
using AgentsRest.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgentsRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MissionController(IMissionService missionService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllMission()
        {
            return Ok(await missionService.GetAllMissions());
        }
        [HttpPut("UpdateMission{id}")]
        public async Task<IActionResult> UpdateMission(int id, [FromBody] StatusDto statusDto )
        {
            try
            {
                await missionService.UpdateMissionAsync(id, statusDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
    }
}

