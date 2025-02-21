using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Pizzashop_dotnet.Services
{

    public class JwtService : IJwtService
    {

        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }


        public string GenerateJwtToken(string name, string email, string role , string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, name),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypes.NameIdentifier , username)
        };

            var token = new JwtSecurityToken(
                issuer : _config["Jwt:Issuer"],
                audience : _config["Jwt:Audience"],
                claims : claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:ExpiryInMinutes"])),
                signingCredentials: credentials);

            var newtoken = new JwtSecurityTokenHandler().WriteToken(token);
            return newtoken;

        }
    }
}