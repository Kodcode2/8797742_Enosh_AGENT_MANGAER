using AgentsRest.Dto;
using System.Runtime.Serialization.Json;
using System.Text.Json;

namespace AgentsRest.Middleware
{
	public class LoginMiddleware(RequestDelegate next)
	{
		private readonly RequestDelegate _next = next;
		public async Task InvokeAsync(HttpContext context)
		{
			var req = context.Request;
			if (req.Path != "/Login")
			{
				using (StreamReader reader = new StreamReader(req.Body))
				{
					var requestBody = await reader.ReadToEndAsync();
					TokenDto token = JsonSerializer.Deserialize<TokenDto>(requestBody,new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
				}
				
			}
			Console.WriteLine(req.Path.ToString());
			await _next(context);
		}
	}
}
