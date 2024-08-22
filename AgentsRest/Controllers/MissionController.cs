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
        [HttpGet("missions")]
        public async Task<IActionResult> GetAllMission()
        {
            return Ok(await missionService.GetAllMissions());
        }
        [HttpPut("missions/{id}")]
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
        [HttpPost("missions/update")]
        public async Task<IActionResult> AgentsChasesTargets()
        {
            try
            {
                await missionService.AgentsChasesTargets();
                return Ok();
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);

            }
        }
    }
}

