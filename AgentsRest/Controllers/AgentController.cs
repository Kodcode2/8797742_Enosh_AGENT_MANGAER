using AgentsRest.Dto;
using AgentsRest.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AgentsRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentController(IAgentService agentService, IMissionService _missionService) : ControllerBase
    {
        [HttpGet("agents")]
        public async Task<IActionResult> GetAllAgent()
        {
            return Ok(await agentService.GetAllAgentAsync());
        }
        [HttpGet("GetAgentById")]
        public async Task<IActionResult> GetAgentById(int id)
        {
            return Ok( await agentService.FindAgentByIdAsync(id));
        }
        [HttpPost("agents")]
        public async Task<IActionResult> CreateNewAgent(AgentDto agentDto)
        {
            try
            {
                var agent = await agentService.CreateNewAgentAsync(agentDto);

                return Created("new agent",agent?.Id);
            }   
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            

        }
        [HttpPut("agents/{id}/pin")]
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
        [HttpPut("agents/{id}/move")]
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
