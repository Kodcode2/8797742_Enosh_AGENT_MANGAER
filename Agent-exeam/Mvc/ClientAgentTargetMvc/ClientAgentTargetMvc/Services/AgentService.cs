using AgentsRest.Models;
using ClientAgentTargetMvc.ViewModels;
using System.Text.Json;

namespace ClientAgentTargetMvc.Services
{
    public class AgentService(IHttpClientFactory clientFactory) : IAgentService
    {
        private readonly string BaseUrl = "https://localhost:7243";

        public async Task<List<AgentVM>> GetALlAgents()
        {
            var httpClient = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/Agents");
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authentication.Token);

            var respounce = await httpClient.SendAsync(request);
            if (respounce.IsSuccessStatusCode)
            {
                var content = await respounce.Content.ReadAsStringAsync();
                List<AgentVM>? agents = JsonSerializer.Deserialize<List<AgentVM>>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return agents;
            }
            throw new Exception("agents not found");
        }
        public async Task<AgentVM> FintAgentById(int id)
        {
            var agents = await GetALlAgents();
            var agent = agents.FirstOrDefault(a => a.Id == id);
            return agent;
        }
    }
}
