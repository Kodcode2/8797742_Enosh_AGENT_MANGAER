namespace AgentsRest.Models
{
    
    public class KillModel
    {
        public int Id { get; set; }

        public string AgentId { get; set; }
        public AgentModel Agent { get; set; }

        public string TargetId { get; set; }

        public TargetModel Target { get; set; }

        public DateTime KillTime { get; set; }
    }
}
