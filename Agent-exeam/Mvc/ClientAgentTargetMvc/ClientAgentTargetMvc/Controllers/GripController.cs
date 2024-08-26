using ClientAgentTargetMvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClientAgentTargetMvc.Controllers
{
    public class GripController(IGridService gridService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var res = await gridService.GetGrid();
            return View(res);
        }
    }
}
