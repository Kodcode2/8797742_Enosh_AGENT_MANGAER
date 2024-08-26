using ClientAgentTargetMvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClientAgentTargetMvc.Controllers
{
    public class AgentsController(IAgentService agentService) : Controller
    {
        public  async Task<IActionResult> Index()
        {
            try
            {
                var res = await agentService.GetALlAgents();
                return View(res);
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var res = await agentService.FintAgentById(id);
                return View(res);
            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }
    }
}
