using AgentsRest.Dto;
using AgentsRest.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace AgentsRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AgentsController(IServiceProvider serviceProvider) : ControllerBase
    { 
        // מייצר גישה לסרביס של משימות
        private IMissionService missionService => serviceProvider.GetRequiredService<IMissionService>();
		// מייצר גישה לסרביס של סוכנים

		private IAgentService agentService => serviceProvider.GetRequiredService<IAgentService>();

        [HttpGet]
        // מביא את כל הסוכנים
        public async Task<IActionResult> GetAllAgent()
        {
            return Ok(await agentService.GetAllAgentAsync());
        }
        [HttpGet("GetAgentById")]
        // מביא סוכן לפי ID
        public async Task<IActionResult> GetAgentById(int id)
        {
            //הולך לפונקציה בסרביס שמחפשת סוכן לפי ID
            return Ok( await agentService.FindAgentByIdAsync(id));
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateNewAgent(AgentDto agentDto)
        {
            //מייצר סוכן חדש
            try
            {
                var agent = await agentService.CreateNewAgentAsync(agentDto);
                return Created("new agent",new IdDto { Id = agent.Id});
            }  // אם אירעה שגיאה שולח שגיאה 
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            

        }
        [HttpPut("{id}/pin")]
        // מעדכן סוכן ומציב אותו בתור מיקןם התחלתי
        public async Task<IActionResult> UpdateAgent(int id, [FromBody] LocationDto locationDto)
        {
            try
            {
                await agentService.UpdateAgentAsync(id, locationDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}/move")]
        // מוזיז סוכן לפי כיוונים
        public async Task<IActionResult> UpdateAgentDirection(int id, [FromBody] DirectionDto directionDto)
        
        {
            try
            {
                await agentService.UpdateAgentDirectionAsync(id, directionDto);

                
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
