namespace AgentsRest.Models
{
    
    public class KillModel
    {
        public int Id { get; set; }

        public int AgentId { get; set; }
        public AgentModel Agent { get; set; }

        public int TargetId { get; set; }

        public TargetModel Target { get; set; }

        public DateTime KillTime { get; set; }
    }
}
