namespace AgentsRest.Service
{
    public interface IJwtService
    {
        string GnerateToken(string serverName);
    }
}
