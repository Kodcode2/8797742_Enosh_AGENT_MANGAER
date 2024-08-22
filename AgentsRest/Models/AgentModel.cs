using Microsoft.EntityFrameworkCore;

namespace AgentsRest.Models
{
    public enum AgentStatus
    {
        Sleping,
        Active
    }




    public class AgentModel
    {
        public int Id { get; set; }

        public required string Image {   get; set; }

        public required string NickName { get; set; }

        public int X { get; set; } = -1;
        public int Y { get; set; } = -1;

        public AgentStatus Status { get; set; } = AgentStatus.Sleping;

       public List<MissionModel> Missions { get; set; } = [];
    }
}
