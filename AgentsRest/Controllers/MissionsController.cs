using AgentsRest.Dto;
using AgentsRest.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace AgentsRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MissionsController(IServiceProvider serviceProvider) : ControllerBase
    {
        private IMissionService missionService => serviceProvider.GetRequiredService<IMissionService>();
        private IAgentService agentService => serviceProvider.GetRequiredService<IAgentService>(); 
        private ITargetService targetService => serviceProvider.GetRequiredService<ITargetService>();

        [HttpGet]
        public async Task<IActionResult> GetAllMission()
        {
            return Ok(await missionService.GetAllMissions());
        }
        [HttpGet("GetMissionInclut")]
        public async Task<IActionResult> GetMissionInclut()
        {
            
            return Ok(await missionService.GetMissionInclut());
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMission(int id, [FromBody] StatusDto statusDto)
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
        [HttpPost("update")]
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
        //public async Task<IActionResult> Front()
        //{
        //    try
        //    {
        //        var a = await agentService.GetAllAgentAsync();
        //        var t = a.Where(s => s.Status == Models.AgentStatus.Active)
        //    }
        //}
    }
}

