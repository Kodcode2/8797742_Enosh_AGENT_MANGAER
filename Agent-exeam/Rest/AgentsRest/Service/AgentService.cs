using AgentsRest.Dto;
using AgentsRest.Models;
using Microsoft.EntityFrameworkCore;
using UserApi.Data;

namespace AgentsRest.Service
{
    public class AgentService(ApplicationDbContext context, IServiceProvider serviceProvider) : IAgentService
    {


        // מביא את הסרביס של משימות
        private IMissionService _missionService => serviceProvider.GetRequiredService<IMissionService>();
        // מייצר סוכן חדש
        public async Task<AgentModel?> CreateNewAgentAsync(AgentDto agentDto)
        {
            // מיועד לבדיקה האם סוכן כבר קיים
            var exists = await context.Agents.Where(x => x.Image == agentDto.PhotoUrl).ToListAsync();
            // אם יש אותו תמונה 
            //if (exists.Count > 0) { throw new Exception($" Agent with the Image {agentDto.PhotoUrl} is alraedy exists"); }
            // לוקח את הסוכן ומייצר ממנו מודל
            AgentModel agentModel = new AgentModel()
            { 
                Image = agentDto.PhotoUrl,
                NickName = agentDto.Nickname
            };
            // מכניס לדאדטאבאיס
            await context.Agents.AddAsync(agentModel);
			// שןמר לדאדטאבאיס
			await context.SaveChangesAsync();
            // מחזיר את הסוכן שייצרתי
            return agentModel;
        }

        public async Task<AgentModel?> FindAgentByIdAsync(int id)
        {
            // מחפש סוכן לפי ID 
           AgentModel? agent =  await context.Agents.FirstOrDefaultAsync(agent => agent.Id == id);
            // אם הסוכן לא קיים מחזיר NULL
            if (agent == null) { return null; }
            // מחזיר את הסוכן
            return agent;
        }

        public async Task<List<AgentModel>> GetAllAgentAsync()
        {
            // מביא את כל הסוכנים מהדאטאבאיס
            return await context.Agents.ToListAsync();
        }

        public async  Task UpdateAgentAsync(int id , LocationDto locationDto)
        {
            // מעדכן מיקום  התחלתי של סוכן
            var agent = await FindAgentByIdAsync(id);
            // אם לא מוצא אותו שולח הערה
            if (agent == null) { throw new Exception($" Agent with the id {id} dosent exists"); }
            // משנה מיקום של סוכן
            agent.X = locationDto.X;
            agent.Y = locationDto.Y;
            // שומר בדאטאבאיס
            await context.SaveChangesAsync();
        }
        // DICT לשמירה של כל הכיוונים
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
        // מוזיז סוכנים  
        public async Task UpdateAgentDirectionAsync(int id, DirectionDto directionDto)
        {
            // מוצא אם קיים
            var agent = await FindAgentByIdAsync(id);
            // אם לא זרק שגיאה
            if (agent == null) { throw new Exception($" Agent with the id {id} dosent exists"); }
            // בודק את הכיוון אם תקין
            bool isExists = deriction.TryGetValue(directionDto.Direction, out var result);
            if (!isExists)
            {
                throw new Exception($" The diraction {directionDto.Direction} is not in Dict");
            }
            // בודק אם הסוכן פעיל
            if(agent.Status == AgentStatus.Active)
            {
                throw new Exception($" The Agent {agent.NickName} is Active");
            }
            var (x, y) = result;

            // הולך לסרבר של משימות בודק אם יש אפשרות לציוות 
            await _missionService.CheakIfHaveMatchAgent(agent,x,y);
            // בודק אם יש משימות שכבר לא רלוונטיות
            await _missionService.CheakIfHaveMissionNotRleventAgent(agent);
            // אם עובר את הכל מעדכן מיקום
            agent.X += x;
            agent.Y += y;
            // שומר שינויים בדאטאבאיס
            await context.SaveChangesAsync();
        }
    }
}
