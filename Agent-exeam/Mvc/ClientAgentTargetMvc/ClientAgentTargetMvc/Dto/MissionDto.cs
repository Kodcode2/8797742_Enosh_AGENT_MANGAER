using AgentsRest.Models;

namespace ClientAgentTargetMvc.Dto
{

    public class MissionDto
    {

        public required string TargetName { get; set; }

        public required string Position { get; set; }

        public required string TargetImage { get; set; }

        public int XTarget { get; set; } = -1;

        public int YTarget { get; set; } = -1;

        public TargetStatus TargetStatus { get; set; } = TargetStatus.Alive;

        public required string AgentImage { get; set; }

        public required string NickName { get; set; }

        public int XAgent { get; set; } = -1;
        public int YAgent { get; set; } = -1;

        public AgentStatus agentStatus { get; set; } = AgentStatus.Sleping;
        public int MissionId { get; set; }

        public int AgentId { get; set; }

        public AgentModel Agent { get; set; }

        public int TargetId { get; set; }

        public TargetModel Target { get; set; }

        public double TimeLeft { get; set; }

        public MissionStatus MissionStatus { get; set; } = MissionStatus.IntialContract;
        public double TimeRight { get; set; }

        public DateTime MissionCompletedTime { get; set; }
        public double Distance { get; set; }
    }
}
