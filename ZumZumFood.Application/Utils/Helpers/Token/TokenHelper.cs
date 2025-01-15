namespace ZumZumFood.Application.Utils.Helpers.Token
{
    public static class TokenHelper
    {
        public static string GenerateJwtToken(int userId, string username, string fullName, string userEmail, IEnumerable<string> roles, IConfiguration configuration)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:key"]);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, userEmail),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.GivenName, fullName),
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

        public static long? GetCurrentUserId(ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)?.Trim();
                if (long.TryParse(userId, out var result))
                {
                    return result;
                }
            }
            return 0;   // Nếu không thể chuyển đổi sang long, trả về null
        }

        public static string? GetCurrentUsername(ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                return user.FindFirstValue(ClaimTypes.Name)?.Trim() ?? "admin";  // Trả về "admin" nếu không có username
            }
            return "admin";  // Trường hợp người dùng không xác thực
        }

        public static string? GetCurrentFullname(ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                return user.FindFirstValue(ClaimTypes.GivenName)?.Trim() ?? "admin";  // Trả về "admin" nếu không có username
            }
            return "admin";  // Trường hợp người dùng không xác thực
        }

        public static string? GetCurrentEmail(ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                return user.FindFirstValue(ClaimTypes.Email)?.Trim();
            }
            return null;
        }

        public static string[] GetCurrentRoles(ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                return user.Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value)
                    .ToArray();
            }
            return new string[0];  // Trả về mảng rỗng nếu không có roles
        }
    }
}
