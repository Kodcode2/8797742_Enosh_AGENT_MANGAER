using ClientAgentTargetMvc.Services;
using ClientAgentTargetMvc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ClientAgentTargetMvc.Controllers
{
    public class ClientHomeController(IHttpClientFactory clientFactory ,IAgentService agentService ,ITargetService targetService , IMissionService missionService) : Controller
    {

        public async Task<IActionResult> Index()
        {
            try
            
            
            {
                return View(await missionService.GetAllMissionVM());
            }
            catch (Exception ex)
            {
               return BadRequest("not found");
            }

        }
        public async Task<IActionResult> Front()
        {
            try


            {
                return View(await missionService.GetAllMissionVM());
            }
            catch (Exception ex)
            {
                return BadRequest("not found");
            }
        }
        public async Task<IActionResult> Frount()
        {
            var frount = await missionService.DashBoardView();

            return View(frount);


        }
        public async Task<IActionResult> Dash()
        {
            var res = await missionService.GetDAshView();
            return View(res);

        }
        public async Task<IActionResult> Zivot(int id)
        {
            try
            {
                var res = await missionService.Zivot(id);
                return RedirectToAction("Details", res);
            }
            catch (Exception ex)
            {
                return View(ex);
            }

        }
    }
}
