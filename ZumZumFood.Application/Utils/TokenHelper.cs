using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ZumZumFood.Application.Utils
{
    public static class TokenHelper
    {
        public static string GenerateJwtToken(int userId, string userEmail, IEnumerable<string> roles, IConfiguration configuration)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = System.Text.Encoding.ASCII.GetBytes(configuration["Jwt:key"]);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, userEmail)
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1), // Token expires after 1 hour
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }
    }
}
