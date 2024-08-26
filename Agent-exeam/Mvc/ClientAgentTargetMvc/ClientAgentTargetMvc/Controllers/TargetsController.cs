using ClientAgentTargetMvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClientAgentTargetMvc.Controllers
{
    public class TargetsController(ITargetService targetService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            try
            {
                var res = await targetService.GetAllTargets();
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
                var res = await targetService.FintTargetById(id);
                return View(res);
            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }
    }
}
