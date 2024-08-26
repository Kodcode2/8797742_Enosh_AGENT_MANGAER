using AgentsRest.Dto;
using AgentsRest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Reflection;
using UserApi.Data;

namespace AgentsRest.Service
{
    public class MissionService(IServiceProvider serviceProvider, ApplicationDbContext context) : IMissionService
    {


        private IAgentService agentService => serviceProvider.GetRequiredService<IAgentService>();
        private ITargetService targetService => serviceProvider.GetRequiredService<ITargetService>();


        public async Task AgentsChasesTargets()
        {

            List<MissionModel> allActiveMission = await context.Missions.Where(x => x.MissionStatus == MissionStatus.InProgres).ToListAsync();
            foreach (MissionModel mission in allActiveMission)
            {
                AgentModel agentModel = await context.Agents.FirstOrDefaultAsync(a => a.Id == mission.AgentId);
                TargetModel targetModel = await context.Targets.FirstOrDefaultAsync(t => t.Id == mission.TargetId);
                AgentModel agent = agentModel;
                TargetModel target = targetModel;
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
            await context.SaveChangesAsync();
        }
        public async Task<bool> CheckCanTerminate(MissionModel mission, AgentModel agent, TargetModel target)
        {
            if (agent.X == target.X && agent.Y == target.Y)
            {
                mission.MissionStatus = MissionStatus.Compleded;
                mission.MissionCompletedTime = DateTime.Now;
                mission.Agent.Status = AgentStatus.Sleping;
                mission.Target.Status = TargetStatus.Eliminated;
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public List<AgentModel> CheackIfHaveAgentIn200(TargetModel target, List<AgentModel> agents) => agents
                            .Where(t =>
                                Math.Sqrt(
                                    Math.Pow(t.X - target.X, 2) +
                                    Math.Pow(t.Y - target.Y, 2)) <= 200
                             ).ToList();
        public List<AgentModel> cheackIfAgentNotActive(List<AgentModel> agents)
        => agents.Where(a => a.Status == AgentStatus.Sleping).ToList();


        public async Task CheakIfHaveMatchTarget(TargetModel target)

        {
            try
            {
                List<AgentModel> agents = await context.Agents.Where(agent => agent.Status == AgentStatus.Sleping).ToListAsync();
                if (agents != null)
                {
                    if (CheackIfHaveAgentIn200(target, agents) != null)
                    {
                        var MissionInProgres = await CheackIfAlradeyExistsTarget(target);
                        if (MissionInProgres.Count != 0)
                        {
                            MissionModel mission = MissionInProgres.Select(m => m).Single();
                            AgentModel Agent = MissionInProgres.Select(m => m.Agent).Single();
                            var res = await CheckCanTerminate(mission, Agent, target);
                            if (!res)
                            {
                                mission.TimeLeft = ReyurnDistance(Agent.X, target.X, Agent.Y, target.Y) / 5; ;
                            }

                        }
                        else
                        {
                            var mission = CheackIfHaveAgentIn200(target, agents);
                            var AgentsNotActive = cheackIfAgentNotActive(mission);
                            if (AgentsNotActive != null)
                            {
                                foreach (var agent in AgentsNotActive)
                                {
                                    await CreateNewMission(agent.Id, target.Id);
                                }
                                //AgentsNotActive.Select(async (t) => await CreateNewMission(target.Id, t.Id));
                            }

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }
        public async Task<List<MissionModel>> CheackIfAlradeyExistsTarget(TargetModel target)
        {
            List<MissionModel> allActiveMission = await context.Missions.Where(m => m.MissionStatus == MissionStatus.InProgres).ToListAsync();
            return allActiveMission.Where(m => m.Id == target.Id).ToList();
        }
        public async Task<bool> CheackIfAlradeyExistsAgent(AgentModel agent)
        {
            List<MissionModel> allActiveMission = await context.Missions.Where(m => m.MissionStatus == MissionStatus.InProgres).ToListAsync();
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
            AgentModel? agent = await context.Agents.FirstOrDefaultAsync(a => a.Id == mission.AgentId);
            if (statusDto.Status == "assigned")
            {
                mission.MissionStatus = MissionStatus.InProgres;
                mission.Agent.Status = AgentStatus.Active;
            }
            await context.SaveChangesAsync();
        }

        public async Task<MissionModel> CreateNewMission(int agentId, int targetId)
        {
            List<MissionModel> MissionList = await context.Missions.Where(x => x.AgentId == agentId && x.TargetId == targetId).Include(a => a.Agent).Include(t => t.Target).ToListAsync();
            if (MissionList.Count > 0)
            {
                return MissionList.FirstOrDefault(m => m.AgentId == agentId && m.TargetId == targetId);
            }
            AgentModel? agent = await context.Agents.FirstOrDefaultAsync(a => a.Id == agentId);
            TargetModel? target = await context.Targets.FirstOrDefaultAsync(t => t.Id == targetId);

            MissionModel model = new MissionModel()
            {
                Agent = agent,
                Target = target,
                AgentId = agentId,
                TargetId = targetId

            };
            await context.Missions.AddAsync(model);
            await context.SaveChangesAsync();
            return model;
        }
        public List<TargetModel> ReturnTargetIn200(AgentModel agent, List<TargetModel> targets)
        => targets
          .Where(t =>
           Math.Sqrt(
           Math.Pow(t.X - agent.X, 2) +
           Math.Pow(t.Y - agent.Y, 2)) <= 200
           ).ToList
            ();

        public async Task CheakIfHaveMatchAgent(AgentModel agent, int x, int y)
        {
            if (!await CheackIfAlradeyExistsAgent(agent))
            {

                if (agent.X + x < 0 || agent.Y + y < 0 || agent.X + x > 1000 || agent.Y + y > 1000)
                {
                    throw new Exception($" You cant go out from the matriza");
                }
                List<TargetModel> targets = await context.Targets.Where(target => target.Status == TargetStatus.Alive).ToListAsync();
                if (ReturnTargetIn200 != null)
                {


                    List<MissionModel> missionModels = await context.Missions.Where(m => m.MissionStatus == MissionStatus.InProgres).ToListAsync();
                    foreach (MissionModel mission in missionModels)
                    {
                        foreach (TargetModel target in targets)
                        {
                            if (mission.TargetId == target.Id)
                            {
                                break;
                            }
                        }
                    }

                    if (targets != null)
                    {
                        var missions = ReturnTargetIn200(agent, targets);
                        missions.Select(async (t) => await CreateNewMission(agent.Id, t.Id));
                    }
                }

            }
            else
            {
                throw new Exception("Agent already active");
            }
        }


        public bool InDistance(int x1, int x2, int y1, int y2)
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
            foreach (MissionModel mission in MissionsOffer)
            {
                TargetModel target = mission.Target;
                if (!InDistance(agent.X, target.X, agent.Y, target.Y))
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
                mission.TimeLeft = ReyurnDistance(agent.X, target.X, agent.Y, target.Y) / 5;
            }
        }
        public async Task CheakIfHaveMissionNotRleventTarget(TargetModel target)
        {
            List<MissionModel> MissionsOffer = await context.Missions.Where(m => m.TargetId == target.Id).Where(m => m.MissionStatus == MissionStatus.IntialContract).Include(a => a.Agent).ToListAsync();
            foreach (MissionModel mission in MissionsOffer)
            {
                AgentModel agent = mission.Agent;
                if (!InDistance(agent.X, target.X, agent.Y, target.Y))
                {
                    context.Missions.Remove(mission);
                    await context.SaveChangesAsync();
                }
            }
            List<MissionModel> MissionsActive = await context.Missions.Where(m => m.TargetId == target.Id).Where(m => m.MissionStatus == MissionStatus.InProgres).Include(a => a.Agent).ToListAsync();
            foreach (MissionModel mission in MissionsOffer)
            {
                AgentModel agent = mission.Agent;
                mission.TimeLeft = ReyurnDistance(agent.X, target.X, agent.Y, target.Y) / 5;
            }
        }

        public async Task<List<MissionDto>> GetMissionInclut()
        {
            var a = await context.Missions.Include(x => x.Agent).Include(x => x.Target).ToListAsync();
            List<MissionDto> missions = new List<MissionDto>();
            foreach (MissionModel mission in a)
            {
                missions.Add(new MissionDto()
                {
                    TargetName = mission.Target.TargetName,
                    Position = mission.Target.Position,
                    TargetImage = mission.Target.Image,
                    XTarget = mission.Target.X,
                    YTarget = mission.Target.Y,
                    TargetStatus = mission.Target.Status,
                    AgentImage = mission.Agent.Image,
                    NickName = mission.Agent.NickName,
                    XAgent = mission.Agent.X,
                    YAgent = mission.Agent.Y,
                    agentStatus = mission.Agent.Status,
                    MissionId = mission.Id,
                    AgentId = mission.AgentId,
                    TargetId = mission.TargetId,
                    TimeLeft = mission.TimeLeft,
                    MissionStatus = mission.MissionStatus,
                    MissionCompletedTime = mission.MissionCompletedTime,
                    Distance = ReyurnDistance(mission.Target.X, mission.Agent.X, mission.Target.Y, mission.Agent.Y)
                });
            }
            return missions;
        }

        public async Task<List<NewMissionDto>> GetMissionInclutAgentTarget()
        {
            var res = await context.Missions.Include(a => a.Agent).Include(t => t.Target).ToListAsync();
            List<NewMissionDto> missions = new();
            missions = res.Select(x => convertMission(x)).ToList();
            return missions;
        }

        public  NewMissionDto convertMission(MissionModel missionModel)

        {
            return new NewMissionDto() 
            { 
                Id = missionModel.Id,
                AgentId = missionModel.AgentId,
                Agent = missionModel.Agent,
                TargetId = missionModel.TargetId,
                Target = missionModel.Target,
                TimeLeft = missionModel.TimeLeft,
                MissionCompletedTime = missionModel.MissionCompletedTime,
                MissionStatus= missionModel.MissionStatus,
                Distance = ReyurnDistance(missionModel.Agent.X,missionModel.Target.X,missionModel.Agent.Y , missionModel.Target.Y),
                
            }
            ;
        }
        
    }
}
