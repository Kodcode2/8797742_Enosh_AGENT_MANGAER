using AgentsRest.Models;
using ClientAgentTargetMvc.Dto;
using ClientAgentTargetMvc.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Signing;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ClientAgentTargetMvc.Services
{
    public class MissionService(IHttpClientFactory 
        clientFactory) : IMissionService
    {
        private readonly string BaseUrl = "https://localhost:7243";

        public async Task<List<MissionVM>> GetAllMissions()
        {

            var httpClient = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/Missions");
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authentication.Token);

            var respounce = await httpClient.SendAsync(request);
            if (respounce.IsSuccessStatusCode)
            {
                var content = await respounce.Content.ReadAsStringAsync();
                List<MissionVM>? missions = JsonSerializer.Deserialize<List<MissionVM>>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return missions;
            }
            throw new Exception("agents not found");
        }
        public async Task<List<MissionDto>> GetAllMissionVM()
        {
            var httpClient = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/Missions/GetMissionInclut");
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authentication.Token);

            var respounce = await httpClient.SendAsync(request);
            if (respounce.IsSuccessStatusCode)
            {
                var content = await respounce.Content.ReadAsStringAsync();
                List<MissionDto>? missions = JsonSerializer.Deserialize<List<MissionDto>>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return missions;
            }
            throw new Exception("agents not found");
        }
        public async Task<FrontView> DashBoardView()
        {
            var allMissionInclud = await GetAllMissionVM();

            var allAgent = allMissionInclud.Select(a => a.Agent).Count();

            var allActiveAgent = allMissionInclud.Where(a => a.agentStatus == AgentsRest.Models.AgentStatus.Active).Count();

            var allTarget = allMissionInclud.Select(a => a.Target).Count();

            var allActiveTarget = allMissionInclud.Where(a => a.TargetStatus == AgentsRest.Models.TargetStatus.Eliminated).Count();

            var allMisissions = allMissionInclud.Count();

            var allActiveMission = allMissionInclud.Where(a => a.MissionStatus == ClientAgentTargetMvc.Dto.MissionStatus.InProgres).Count();

            int agentToTarget = allTarget / allTarget;

            int sumAgentsOferToTargets = allMissionInclud.Where(m => m.MissionStatus == ClientAgentTargetMvc.Dto.MissionStatus.IntialContract)
                .Where(a => a.agentStatus == AgentsRest.Models.AgentStatus.Sleping)
                .GroupBy(a => a.Agent).Count();

            FrontView frontView = new()
            {
                AllAgent = allAgent,
                AllActiveAgent = allActiveAgent,
                AllTarget = allTarget,
                AllActiveTarget = allActiveTarget,
                AllMission = allMisissions,
                AllActiveMission = allActiveMission,
                AgentToTarget = agentToTarget,
                SumAgentsOferToTargets = sumAgentsOferToTargets
            };
            return frontView;
        }

        public async Task<MissionVM> FintMissionById(int id)
        {
            var missions = await GetAllMissions();
            var mission = missions.FirstOrDefault(m => m.Id == id);
            return mission;
        }

        public async Task<MissionVM>  Zivot(int id)
        {
            var httpClient = clientFactory.CreateClient();

            var httpContent = new StringContent(
                JsonSerializer.Serialize(new
                {
                    status = "assigned"
                }),
                Encoding.UTF8,
                "application/json"
            );
            var request = new HttpRequestMessage(HttpMethod.Put, $"{BaseUrl}/Missions/{id}");
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authentication.Token);
            request.Content = httpContent;
            var respounce = await httpClient.SendAsync(request);
            if (respounce.IsSuccessStatusCode)
            {
                return await FintMissionById(id);
            }
            return null;
        }
        public async Task<List<NewMissionDto>> GetDAshView()
        {
            var httpClient = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/Missions/MissionInclutAgentTarget");
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authentication.Token);

            var respounce = await httpClient.SendAsync(request);
            if (respounce.IsSuccessStatusCode)
            {
                var content = await respounce.Content.ReadAsStringAsync();
                List<NewMissionDto>? missions = JsonSerializer.Deserialize<List<NewMissionDto>>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                var MissionsInOfer = missions.Where(x => x.MissionStatus == ClientAgentTargetMvc.Dto.MissionStatus.IntialContract);
                return MissionsInOfer.ToList(); 
            }
            throw new Exception("Mission not found");
        }
        

    }
}
