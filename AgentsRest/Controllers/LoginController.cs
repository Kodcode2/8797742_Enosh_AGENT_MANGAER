using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using AgentsRest.Dto;
using AgentsRest.Service;
using Microsoft.Extensions.DependencyInjection;

namespace AgentsRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public class LoginClientController(IHttpClientFactory clientFactory, IJwtService _jwtService) : ControllerBase
        {
            
            private readonly string loginApi = "https://localhost:7243";

            [HttpPost]
            public async Task<ActionResult<string>> Login()
            {
                var httpClient = clientFactory.CreateClient();
                var request = new HttpRequestMessage(HttpMethod.Post, loginApi);

                request.Content = new StringContent(
                    JsonSerializer.Serialize(new LoginDto() { IdName = "enosh" }),
                    Encoding.UTF8,
                    "application/json"
                );
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    HttpContext.Session.SetString("JwtToken", content);
                    return Ok(content);
                }
                return BadRequest("Login failure");
            }
        }
        //[HttpPost("auth")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //public async Task<ActionResult<string>> Auth([FromBody] LoginDto loginDto)
        //{
        //    try
        //    {
        //        string idName = loginDto.IdName;
                
        //        _jw

        //    }
        //    catch (Exception ex)
        //    {
        //        return Unauthorized(ex.Message);
        //    }
        //}
    }
}
