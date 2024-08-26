using AgentsRest.Models;
using ClientAgentTargetMvc.ViewModels;

namespace ClientAgentTargetMvc.Services
{
    public interface IAgentService
    {
        Task<List<AgentVM>> GetALlAgents();
        Task<AgentVM> FintAgentById(int id);
    }
}
