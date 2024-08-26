namespace AgentsRest.Service
{
	public class LoginService(IJwtService jwtService) : ILoginService
	{
		
		static	List<string> AutoriseServers = ["SimulationServer", "MvcServer"];

		public string Login(string serverName)
		{
			if (AutoriseServers.Contains(serverName))
			{
				return jwtService.GnerateToken(serverName);
			}
			throw new Exception("this server is unautorise");
		}
	}
}
