using AgentsRest.Dto;
using AgentsRest.Models;
using Microsoft.EntityFrameworkCore;
using UserApi.Data;

namespace AgentsRest.Service
{
    public class MissionService : IMissionService
    {

        private readonly ApplicationDbContext _context;
        public MissionService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<MissionModel>> GetAllMissions()
        {
            return await _context.Missions.ToListAsync();
        }

        public async Task UpdateMissionAsync(int id, StatusDto statusDto)
        {
            var mission = await _context.Missions.FirstOrDefaultAsync(mission  => mission.Id == id);
            if (mission == null) { throw new Exception($" mission with the id {id} dosent exists"); }
            if (statusDto.Status == "assigned")
            {
                mission.MissionStatus = MissionStatus.IntialContract;
                    }
            await _context.SaveChangesAsync();
        }
    }
}
