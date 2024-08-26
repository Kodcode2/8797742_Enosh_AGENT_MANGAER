using AgentsRest.Models;
using ClientAgentTargetMvc.Dto;
using ClientAgentTargetMvc.ViewModels;

namespace ClientAgentTargetMvc.Services
{
    public interface IMissionService
    {
        Task<List<MissionVM>> GetAllMissions();
        Task<List<MissionDto>> GetAllMissionVM();

        Task<MissionVM> FintMissionById(int id);

        Task<FrontView> DashBoardView();

        Task<MissionVM> Zivot(int id);

        Task<List<NewMissionDto>> GetDAshView();
    }
}
