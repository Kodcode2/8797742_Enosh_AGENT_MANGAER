using AgentsRest.Dto;
using AgentsRest.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgentsRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TargetController(ITargetService targetService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllTargetAsync()
        {
            return Ok(await targetService.GetAllTargets());
        }
        [HttpGet("GetAgentById")]
        public async Task<IActionResult> GetAgentById(int id)
        {
            return Ok(await targetService.FindTargetByIdAsync(id));
        }

        [HttpPost("CreateNewTarget")]
        public async Task<IActionResult> CreateNewTarget(TargetDto targetDto)
        {
            try
            {
                var target = await targetService.CreateNewTargetAsync(targetDto);

                return Created("new agent", target?.Id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
        [HttpPut("UpdateTarget{id}/pin")]
        public async Task<IActionResult> UpdateTarget(int id, [FromBody] LocationDto locationDto)
        {
            try
            {
                await targetService.UpdateTargetAsync(id, locationDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("UpdateTarget{id}/move")]
        public async Task<IActionResult> UpdateTargetDirection(int id, [FromBody] DirectionDto directionDto)
        {
            try
            {
                await targetService.UpdateTargetDirectionAsync(id, directionDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
