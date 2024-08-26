using AgentsRest.Models;
using ClientAgentTargetMvc.ViewModels;

namespace ClientAgentTargetMvc.Services
{
    public interface ITargetService
    {
        Task<List<TargetVM>> GetAllTargets();

        Task<TargetVM> FintTargetById(int id);

    }
}
