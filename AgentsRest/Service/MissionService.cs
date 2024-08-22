using AgentsRest.Dto;
using AgentsRest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using UserApi.Data;

namespace AgentsRest.Service
{
    public class MissionService(IAgentService agentService, IServiceProvider serviceProvider, ApplicationDbContext context) : IMissionService
    {
        

        private IAgentService agentService = serviceProvider.GetRequiredService<IAgentService>();
        private ITargetService targetService = serviceProvider.GetRequiredService<ITargetService>();


        public async Task AgentsChasesTargets()
        {
            
            List<MissionModel> allActiveMission = await context.Missions.Where(x => x.MissionStatus == MissionStatus.InProgres).ToListAsync();
            foreach (MissionModel mission in allActiveMission)
            {
                AgentModel agent = mission.Agent;
                TargetModel target = mission.Target;
                await RunAfterTarget(mission, agent, target);
            }
        }
        public async Task RunAfterTarget(MissionModel mission, AgentModel agent, TargetModel target)
        {
            agent.X = target.X > agent.X ? agent.X + 1 : agent.X;
            agent.Y = target.Y > agent.Y ? agent.Y + 1 : agent.Y;
            agent.X = target.X < agent.X ? agent.X - 1 : agent.X;
            agent.Y = target.Y < agent.Y ? agent.Y - 1 : agent.Y;

            if (await CheckCanTerminate(mission, agent, target))
            {
                return;
            };
        }
        public async Task<bool> CheckCanTerminate(MissionModel mission, AgentModel agent, TargetModel target)
        {
            if (agent.X == target.X && agent.Y == target.Y)
            {
                mission.MissionStatus = MissionStatus.Compleded;
                mission.MissionCompletedTime = DateTime.Now;
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task CheakIfHaveMatchTarget(TargetModel target)

        {
            if (!await CheackIfAlradeyExistsTarget(target))
            {

                List<AgentModel> agents = await context.Agents.Where(agent => agent.Status == AgentStatus.Sleping).ToListAsync();

                var mission = agents
                    .Where(t =>
                        Math.Sqrt(
                            Math.Pow(t.X - target.X, 2) +
                            Math.Pow(t.Y - target.Y, 2)) <= 200
                     )
                    .Select(async (t) => await CreateNewMission(target.Id, t.Id));
            }

        }
        public async Task<bool> CheackIfAlradeyExistsTarget(TargetModel target)
        {
            List<MissionModel> allActiveMission = await context.Missions.ToListAsync();
            return allActiveMission.Any(m => m.Id == target.Id);
        }
        public async Task<bool> CheackIfAlradeyExistsAgent(AgentModel agent)
        {
            List<MissionModel> allActiveMission = await context.Missions.ToListAsync();
            return allActiveMission.Any(m => m.Id == agent.Id);
           
        }

        public async Task<List<MissionModel>> GetAllMissions()
        {
            return await context.Missions.ToListAsync();
        }

        public async Task UpdateMissionAsync(int id, StatusDto statusDto)
        {
            var mission = await context.Missions.FirstOrDefaultAsync(mission => mission.Id == id);
            if (mission == null) { throw new Exception($" mission with the id {id} dosent exists"); }
            if (statusDto.Status == "assigned")
            {
                mission.MissionStatus = MissionStatus.IntialContract;
            }
            await context.SaveChangesAsync();
        }

        public async Task<MissionModel> CreateNewMission(int agentId, int targetId)
        {
            var exsists = await context.Missions.FirstOrDefaultAsync(x => x.AgentId == agentId && x.TargetId == targetId);
            if (exsists != null)
            {
                return exsists;
            }
            MissionModel model = new MissionModel()
            {
                AgentId = agentId,
                TargetId = targetId
            };
            await context.Missions.AddAsync(model);
            await context.SaveChangesAsync();
            return model;
        }

        public async Task CheakIfHaveMatchAgent(AgentModel agent)
        {
            if (!await CheackIfAlradeyExistsAgent(agent))
            {
                List<TargetModel> targets = await context.Targets.Where(target => target.Status == TargetStatus.Alive).ToListAsync();

                var missions = targets
                    .Where(t =>
                        Math.Sqrt(
                            Math.Pow(t.X - agent.X, 2) +
                            Math.Pow(t.Y - agent.Y, 2)) <= 200
                     )
                    .Select(async (t) => await CreateNewMission(agent.Id, t.Id));
            }
        }
        

        public bool InDistance(int x1,int x2 , int y1, int y2)
        {
            return Math.Sqrt(
                            Math.Pow(x1 - x2, 2) +
                            Math.Pow(y1 - y2, 2)) <= 200;
        }
        public double ReyurnDistance(int x1, int x2, int y1, int y2)
        {
            return Math.Sqrt(
                            Math.Pow(x1 - x2, 2) +
                            Math.Pow(y1 - y2, 2));
        }

        public async Task CheakIfHaveMissionNotRleventAgent(AgentModel agent)
        {
            List<MissionModel> MissionsOffer = await context.Missions.Where(m => m.AgentId == agent.Id).Where(m => m.MissionStatus == MissionStatus.IntialContract).ToListAsync();
            foreach(MissionModel mission in MissionsOffer)
            {
                TargetModel target = mission.Target;
                if (! InDistance(agent.X , target.X ,agent.Y , target.Y))
                {
                  context.Missions.Remove(mission);
                  await context.SaveChangesAsync();
                }
            }
            List<MissionModel> MissionsActive = await context.Missions.Where(m => m.AgentId == agent.Id).Where(m => m.MissionStatus == MissionStatus.InProgres).ToListAsync();
            foreach (MissionModel mission in MissionsOffer)
            {
                TargetModel target = mission.Target;
                if (target.Status == TargetStatus.Eliminated)
                {
                    mission.MissionStatus = MissionStatus.Compleded;
                    mission.MissionCompletedTime = DateTime.Now;
                    await context.SaveChangesAsync();
                }
                mission.TimeLeft = ReyurnDistance(agent.X, target.X, agent.Y, target.Y) / 5 ;
            }
        }
        public async Task CheakIfHaveMissionNotRleventTarget(TargetModel target)
        {
            List<MissionModel> MissionsOffer = await context.Missions.Where(m => m.TargetId == target.Id).Where(m => m.MissionStatus == MissionStatus.IntialContract).ToListAsync();
            foreach (MissionModel mission in MissionsOffer)
            {
                AgentModel agent = mission.Agent;
                if (!InDistance(agent.X, target.X, agent.Y, target.Y))
                {
                    context.Missions.Remove(mission);
                    await context.SaveChangesAsync();
                }
            }
            List<MissionModel> MissionsActive = await context.Missions.Where(m => m.TargetId == target.Id).Where(m => m.MissionStatus == MissionStatus.InProgres).ToListAsync();
            foreach (MissionModel mission in MissionsOffer)
            {
                AgentModel agent = mission.Agent;
                mission.TimeLeft = ReyurnDistance(agent.X, target.X, agent.Y, target.Y) / 5;
            }
        }
    }
}
