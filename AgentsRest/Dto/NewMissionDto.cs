using AgentsRest.Models;

namespace AgentsRest.Dto
{

    
    public class NewMissionDto
    {

        public int Id { get; set; }

        public int AgentId { get; set; }

        public AgentModel Agent { get; set; }

        public int TargetId { get; set; }

        public TargetModel Target { get; set; }

        public double TimeLeft { get; set; }

        public MissionStatus MissionStatus { get; set; } = MissionStatus.IntialContract;

        public DateTime MissionCompletedTime { get; set; }

        public double Distance {  get; set; }
    }
}