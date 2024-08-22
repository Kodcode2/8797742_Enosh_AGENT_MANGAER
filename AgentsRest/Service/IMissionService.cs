using AgentsRest.Dto;
using AgentsRest.Models;

namespace AgentsRest.Service
{
    public interface IMissionService
    {
        Task<List<MissionModel>> GetAllMissions();

        Task UpdateMissionAsync(int id, StatusDto statusDto);

        Task  AgentsChasesTargets();

        Task CheakIfHaveMatchTarget(TargetModel target);

        Task CheakIfHaveMatchAgent(AgentModel agent);

        Task CheakIfHaveMissionNotRleventAgent(AgentModel agent);

        Task CheakIfHaveMissionNotRleventTarget(TargetModel target);
        // Task<MissionModel> CreateNewMission(int agentId, int targetId);
    }
}
