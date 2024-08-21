namespace AgentsRest.Models
{
    public enum Status
    {
        IsActive = 1,
        IsInactive = 0
    }




    public class AgentModel
    {
        public int Id { get; set; }

        public string Image {   get; set; }

        public string NickName { get; set; }

        public int X { get; set; } = -1;
        public int Y { get; set; } = -1;

        public int Status { get; set; }

    }
}
