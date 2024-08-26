using ClientAgentTargetMvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using UserApi.Models;

namespace ClientAgentTargetMvc.Controllers
{
    public class HomeController(IHttpClientFactory clientFactory) : Controller
    {

        private readonly string BaseUrl = "https://localhost:7243/login";


		public IActionResult Index()
        {
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
