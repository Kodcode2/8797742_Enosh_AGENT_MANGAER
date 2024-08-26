using AgentsRest.Models;
using ClientAgentTargetMvc.ViewModels;

namespace ClientAgentTargetMvc.Services
{
    public interface IGridService
    {
        Task<(List<AgentVM>, List<TargetVM>)> GetGrid();

    }
}
