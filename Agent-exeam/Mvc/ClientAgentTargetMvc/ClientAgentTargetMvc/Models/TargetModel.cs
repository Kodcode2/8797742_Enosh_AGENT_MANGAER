namespace AgentsRest.Models
{
    public enum TargetStatus
    {
        Alive,
        Eliminated
    }
    public class TargetModel
    {
        public int Id { get; set; }

        public required string  TargetName { get; set; }

        public required string Position { get; set; }

        public required string Image { get; set; }

        public int X { get; set; } = -1;

        public int Y { get; set; } = -1;

        public TargetStatus Status { get; set; } = TargetStatus.Alive;
    }
}
