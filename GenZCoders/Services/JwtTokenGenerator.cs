using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GenZCoders.Services
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(
            long accountId,
            string email,
            long roleId,
             string roleName,
            string businessEntity)
        {
            var claims = new List<Claim>
            {
            new Claim("AccountId", accountId.ToString()),
            new Claim(ClaimTypes.Email, email),
            //new Claim(ClaimTypes.Role, roleId.ToString()),
            new Claim(ClaimTypes.Role, roleName),
            new Claim("RoleId", roleId.ToString()),
            new Claim("BusinessEntity", businessEntity)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
