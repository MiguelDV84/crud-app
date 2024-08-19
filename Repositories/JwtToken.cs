using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace crud_app.Repositories
{
    public class JwtToken : IJwtToken
    {
        public IConfiguration configuration;

        public JwtToken(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string TokenGenerator(string email, string fullname)
        {
            var claims = new[]
            {
                new Claim("email", email),
                new Claim("fullname", fullname)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetSection("ConfigurationJwt:Key").Get<string>() ?? string.Empty));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
