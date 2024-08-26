using AgentsRest.Dto;
using AgentsRest.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgentsRest.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class LoginController(ILoginService loginService) : ControllerBase
	{
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public ActionResult Login([FromBody] LoginDto loginDto)
		{
			try
			{
				TokenDto token = new() { token = loginService.Login(loginDto.id) };
				return Ok(token);
			}
			catch (Exception ex)
			{
				return Unauthorized(ex.Message);
			}
		}

	}
}
