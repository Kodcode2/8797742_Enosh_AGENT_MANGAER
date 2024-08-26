using ClientAgentTargetMvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClientAgentTargetMvc.Controllers
{
    public class MissionsController(IMissionService missionService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            try
            {
                var res = await missionService.GetAllMissions();
                return View(res);
            }
            catch (Exception ex)
            {
                return View(ex);

            }
        }
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var res = await missionService.FintMissionById(id);
                return View(res);


            }
            catch (Exception ex)
            {
                return View(ex);
            }
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
