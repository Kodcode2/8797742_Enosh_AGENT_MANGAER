using AgentsRest.Dto;
using AgentsRest.Models;

namespace AgentsRest.Service
{
    public interface IAgentService
    {
        Task<List<AgentModel>> GetAllAgentAsync();

        Task<AgentModel?> FindAgentByIdAsync(int id);

        Task<AgentModel?> CreateNewAgentAsync(AgentDto agentDto);

        Task UpdateAgentAsync(int id, LocationDto locationDto);

    }
}
