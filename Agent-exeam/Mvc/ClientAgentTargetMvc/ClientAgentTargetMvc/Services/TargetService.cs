using AgentsRest.Models;
using ClientAgentTargetMvc.ViewModels;
using System.Text.Json;

namespace ClientAgentTargetMvc.Services
{
    public class TargetService(IHttpClientFactory clientFactory) : ITargetService
    {
        private readonly string BaseUrl = "https://localhost:7243";

        

        public async Task<List<TargetVM>> GetAllTargets()
        {
            var httpClient = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/Targets");
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authentication.Token);

            var respounce = await httpClient.SendAsync(request);
            if (respounce.IsSuccessStatusCode)
            {
                var content = await respounce.Content.ReadAsStringAsync();
                List<TargetVM>? targets = JsonSerializer.Deserialize<List<TargetVM>>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return targets;
            }
            throw new Exception("targets not found");
        }
        public async Task<TargetVM> FintTargetById(int id)
        {
            var targets = await GetAllTargets();
            var target = targets.FirstOrDefault(a => a.Id == id);
            return target;
        }
    }
}
