using AgentsRest.Dto;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AgentsRest.Service
{
    public class JwtService(IConfiguration configuration) : IJwtService
    {

        public string CreateToken(string password)
        {
            string? key = configuration.GetValue<string?>("Jwt:Key", null)
                ?? throw new ArgumentNullException("Invalid JWT key configuration");

            int expiration = configuration.GetValue("Jwt:Expiry", 60);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            Claim[] claims = [new(ClaimTypes.NameIdentifier, password)];

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(expiration),
                claims: claims,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private static readonly ImmutableList<string> allowedNames = [
            "enosh", "avi"
        ];
        public string AuthenticateIdAsync( string idIdentify)
        {
            string identifyName = allowedNames.Contains(idIdentify) ? (CreateToken(idIdentify)) : throw new Exception("Wrong identify");
                ;
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(idIdentify,identifyName);
            if (!isValidPassword) { throw new Exception("Wrong identify"); }
            return idIdentify;

        }
    }
}
