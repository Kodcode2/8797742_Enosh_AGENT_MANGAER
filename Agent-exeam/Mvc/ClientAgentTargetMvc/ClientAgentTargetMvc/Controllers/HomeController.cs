using ClientAgentTargetMvc.Dto;
using ClientAgentTargetMvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using UserApi.Models;

namespace ClientAgentTargetMvc.Controllers
{
    public class HomeController(IHttpClientFactory clientFactory,Authentication authentication) : Controller
    {

        private readonly string BaseUrl = "https://localhost:7243/login";


		public async Task<IActionResult> Index()
        {
			LoginDto loginDto = new LoginDto() { id = "MvcServer" };
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
				return RedirectToAction("Index");
			}
			
			return View();
        }

		

		public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
