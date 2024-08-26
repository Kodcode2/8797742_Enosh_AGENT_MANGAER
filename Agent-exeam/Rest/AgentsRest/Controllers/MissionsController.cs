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
		// מייצר גישה לסרביס של משימות

		private IMissionService missionService => serviceProvider.GetRequiredService<IMissionService>();
		// מייצר גישה לסרביס של סוכנים

		private IAgentService agentService => serviceProvider.GetRequiredService<IAgentService>();
		// מייצר גישה לסרביס של מטרות

		private ITargetService targetService => serviceProvider.GetRequiredService<ITargetService>();

        [HttpGet]
        // מביא את כל המשימות
        public async Task<IActionResult> GetAllMission()
        {
            return Ok(await missionService.GetAllMissions());
        }
        [HttpGet("GetMissionInclut")]
        //  ל VIEW בהתחלה שייצרתימביא משימות כולל סוכנים ומטרות 
        public async Task<IActionResult> GetMissionInclut()
        {
            
            return Ok(await missionService.GetMissionInclut());
        }
        [HttpGet("MissionInclutAgentTarget")]
		// מביא משימות כולל סוכנים ומטרות 

		public async Task<IActionResult> GetMissionInclutAgentTarget()
        {
               var res =  await missionService.GetMissionInclutAgentTarget();
               return Ok(res);
        }
        [HttpPut("{id}")]
        // עדכון סטטוס משימה 
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

