﻿using AgentsRest.Models;

namespace ClientAgentTargetMvc.ViewModels
{
    public enum MissionStatus
    {
        IntialContract,
        InProgres,
        Compleded
    }
    public class MissionVM
    {
        public int Id { get; set; }

        public int AgentId { get; set; }

        public AgentModel Agent { get; set; }

        public int TargetId { get; set; }

        public TargetModel Target { get; set; }

        public double TimeLeft { get; set; }

        public MissionStatus MissionStatus { get; set; } = MissionStatus.IntialContract;

        public DateTime MissionCompletedTime { get; set; }
    }
}
