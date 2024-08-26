using AgentsRest.Dto;
using AgentsRest.Models;
using AgentsRest.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AgentsRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TargetsController(IServiceProvider serviceProvider) : ControllerBase
    {
         private ITargetService targetService => serviceProvider.GetRequiredService<ITargetService>();

    private IMissionService missionService => serviceProvider.GetRequiredService<IMissionService>();


         [HttpGet]
        public async Task<IActionResult> GetAllTargetAsync()
        {
            return Ok(await targetService.GetAllTargetsAsync());
        }
        [HttpGet("GetTargetById")]
        public async Task<IActionResult> GetAgentById(int id)
        {
            return Ok(await targetService.FindTargetByIdAsync(id));
        }

        [HttpPost]
		[Authorize]

		public async Task<IActionResult> CreateNewTarget(TargetDto targetDto)
        {
            try
            {
                var target = await targetService.CreateNewTargetAsync(targetDto);

                return Created("new agent", new IdDto { Id = target.Id});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
        [HttpPut("{id}/pin")]
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
        [HttpPut("{id}/move")]
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
