using AgentsRest.Dto;
using AgentsRest.Models;

namespace AgentsRest.Service
{
    public interface IMissionService
    {
        Task<List<MissionModel>> GetAllMissions();

        Task UpdateMissionAsync(int id, StatusDto statusDto);
    }
}
