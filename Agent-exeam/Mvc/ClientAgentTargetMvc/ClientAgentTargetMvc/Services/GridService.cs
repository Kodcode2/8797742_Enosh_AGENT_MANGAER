using AgentsRest.Models;
using ClientAgentTargetMvc.ViewModels;

namespace ClientAgentTargetMvc.Services
{
    public class GridService(IAgentService agentService , ITargetService targetService) : IGridService
    {
        public async Task<(List<AgentVM>, List<TargetVM>)> GetGrid()
        {
            var agents = await agentService.GetALlAgents();
            var targets = await targetService.GetAllTargets();
            return (agents, targets);
        }
    }
}
