using ClientAgentTargetMvc.Dto;
using ClientAgentTargetMvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using UserApi.Models;

namespace ClientAgentTargetMvc.Controllers
{
	public class LoginController(IHttpClientFactory clientFactory,Authentication authentication) : Controller
	{
		private readonly string BaseUrl = "https://localhost:7243/";

		public IActionResult Index()
		{
			return View(new LoginDto());
		}
		
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Index(LoginDto loginDto)
		{
			var httpClient = clientFactory.CreateClient();

			var httpContent = new StringContent(
				JsonSerializer.Serialize(loginDto),
				Encoding.UTF8,
				"application/json"
);
			var result = await httpClient.PostAsync($"{BaseUrl}/login", httpContent);

			if (result.IsSuccessStatusCode)
			{
				var content = await result.Content.ReadAsStringAsync();
				authentication.Token = content;
				return RedirectToAction("Index","ClientHome");
			}
			return View("AutoEror");
		}
	}
}
