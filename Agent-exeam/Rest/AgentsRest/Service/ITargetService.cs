using AgentsRest.Dto;
using AgentsRest.Models;

namespace AgentsRest.Service
{
    public interface ITargetService
    {
        Task<List<TargetModel>> GetAllTargetsAsync();

        Task<TargetModel?> FindTargetByIdAsync(int id);

        Task<TargetModel?> CreateNewTargetAsync(TargetDto targetDto);

        Task UpdateTargetAsync(int id , LocationDto locationDto);

        Task UpdateTargetDirectionAsync(int id , DirectionDto directionDto);
    }
}
