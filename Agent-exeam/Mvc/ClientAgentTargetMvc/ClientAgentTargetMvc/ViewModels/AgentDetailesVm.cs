namespace ClientAgentTargetMvc.ViewModels
{
    public class AgentDetailesVm
    {
        public AgentVM agentVM { get; set; }

        public int x {  get; set; }
        public int y { get; set; }

        public AgentStatus status { get; set; }

        public MissionVM mission { get; set; }

        public double TimeLeft { get; set; }

        public int AllElimnates { get; set; }

    }
}
