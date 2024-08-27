using AgentsRest.Service;
using Microsoft.AspNetCore.Mvc;

namespace AgentsRest.Controllers
{
    public class ZivotMissionController(IAgentService agentService , ITargetService targetService , IMissionService missionService) : Controller
    {
        //  נועד לבדיקה
        public IActionResult Index()
        {
            var agents = agentService.GetAllAgentAsync();
            var targets = targetService.GetAllTargetsAsync();
            var missions = missionService.GetAllMissions();

            return View();
        }
    }
}
