using AgentsRest.Dto;
using AgentsRest.Service;
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
        private IMissionService missionService => serviceProvider.GetRequiredService<IMissionService>();
        private IAgentService agentService => serviceProvider.GetRequiredService<IAgentService>();

        [HttpGet]
        public async Task<IActionResult> GetAllAgent()
        {
            return Ok(await agentService.GetAllAgentAsync());
        }
        [HttpGet("GetAgentById")]
        public async Task<IActionResult> GetAgentById(int id)
        {
            return Ok( await agentService.FindAgentByIdAsync(id));
        }
        [HttpPost]
        public async Task<IActionResult> CreateNewAgent(AgentDto agentDto)
        {
            try
            {
                var agent = await agentService.CreateNewAgentAsync(agentDto);
                return Created("new agent",new IdDto { Id = agent.Id});
            }   
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            

        }
        [HttpPut("{id}/pin")]
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
