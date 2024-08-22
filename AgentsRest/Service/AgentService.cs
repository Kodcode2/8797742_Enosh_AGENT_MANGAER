using AgentsRest.Dto;
using AgentsRest.Models;
using Microsoft.EntityFrameworkCore;
using UserApi.Data;

namespace AgentsRest.Service
{
    public class AgentService : IAgentService
    {
        private readonly ApplicationDbContext _context;
        public AgentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AgentModel?> CreateNewAgentAsync(AgentDto agentDto)
        {
            var exists = await _context.Agents.FirstOrDefaultAsync(x => x.Image == agentDto.Photo_url);
            if (exists != null) { throw new Exception($" Agent with the Image {agentDto.Photo_url} is alraedy exists"); }
            AgentModel agentModel = new AgentModel()
            { 
                Image = agentDto.Photo_url,
                NickName = agentDto.Nickname
            };
            await _context.Agents.AddAsync(agentModel);
            await _context.SaveChangesAsync();
            return agentModel;
        }

        public async Task<AgentModel?> FindAgentByIdAsync(int id)
        {
           AgentModel? agent =  await _context.Agents.FirstOrDefaultAsync(agent => agent.Id == id);
            if (agent == null) { return null; }
            return agent;
        }

        public async Task<List<AgentModel>> GetAllAgentAsync()
        {
            return await _context.Agents.ToListAsync();
        }

        public async  Task UpdateAgentAsync(int id , LocationDto locationDto)
        {
            var agent = await FindAgentByIdAsync(id);
            if (agent == null) { throw new Exception($" Agent with the id {id} dosent exists"); }
            agent.X = locationDto.X;
            agent.Y = locationDto.Y;
            await _context.SaveChangesAsync();
        }
    }
}
