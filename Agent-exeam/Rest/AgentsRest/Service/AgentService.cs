using AgentsRest.Dto;
using AgentsRest.Models;
using Microsoft.EntityFrameworkCore;
using UserApi.Data;

namespace AgentsRest.Service
{
    public class AgentService(ApplicationDbContext context, IServiceProvider serviceProvider) : IAgentService
    {



        private IMissionService _missionService => serviceProvider.GetRequiredService<IMissionService>();

        public async Task<AgentModel?> CreateNewAgentAsync(AgentDto agentDto)
        {
            var exists = await context.Agents.Where(x => x.Image == agentDto.PhotoUrl).ToListAsync();
            //if (exists.Count > 0) { throw new Exception($" Agent with the Image {agentDto.PhotoUrl} is alraedy exists"); }
            AgentModel agentModel = new AgentModel()
            { 
                Image = agentDto.PhotoUrl,
                NickName = agentDto.Nickname
            };
            await context.Agents.AddAsync(agentModel);
            await context.SaveChangesAsync();
            return agentModel;
        }

        public async Task<AgentModel?> FindAgentByIdAsync(int id)
        {
           AgentModel? agent =  await context.Agents.FirstOrDefaultAsync(agent => agent.Id == id);
            if (agent == null) { return null; }
            return agent;
        }

        public async Task<List<AgentModel>> GetAllAgentAsync()
        {
            return await context.Agents.ToListAsync();
        }

        public async  Task UpdateAgentAsync(int id , LocationDto locationDto)
        {
            var agent = await FindAgentByIdAsync(id);
            if (agent == null) { throw new Exception($" Agent with the id {id} dosent exists"); }
            agent.X = locationDto.X;
            agent.Y = locationDto.Y;
            await context.SaveChangesAsync();
        }
        private readonly Dictionary<string, (int, int)> deriction = new()
        {
            {"nw" , (-1,1) },
            {"n" , (0,1) },
            { "ne" , (1,1) },
            { "w" , (-1,0) },
            { "e" , (1,0) },
            { "sw" , (-1,-1) },
            { "s" , (0,-1) },
            { "se" , (1,-1) },
        };

        public async Task UpdateAgentDirectionAsync(int id, DirectionDto directionDto)
        {
            var agent = await FindAgentByIdAsync(id);
            if (agent == null) { throw new Exception($" Agent with the id {id} dosent exists"); }
            bool isExists = deriction.TryGetValue(directionDto.Direction, out var result);
            if (!isExists)
            {
                throw new Exception($" The diraction {directionDto.Direction} is not in Dict");
            }
            if(agent.Status == AgentStatus.Active)
            {
                throw new Exception($" The Agent {agent.NickName} is Active");
            }
            var (x, y) = result;

            
            await _missionService.CheakIfHaveMatchAgent(agent,x,y);
            await _missionService.CheakIfHaveMissionNotRleventAgent(agent);

            agent.X += x;
            agent.Y += y;
            await context.SaveChangesAsync();
        }
    }
}
