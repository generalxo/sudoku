using Microsoft.IdentityModel.Tokens;
using Sudoku.ApiService.Models.DbModels;
using Sudoku.ApiService.Repos.Interfaces;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Sudoku.ApiService.Repos.Repositories
{
    public class TokenRepositoy : ITokenRepositoy
    {
        private readonly IConfiguration _configuration;
        private const string JwtKey = "Jwt:Key";
        private const string JwtIssuer = "Jwt:Issuer";
        private const string JwtAudience = "Jwt:Audience";
        private readonly JwtSecurityTokenHandler _tokenHandler = new();

        public TokenRepositoy(IConfiguration configuration, JwtSecurityTokenHandler tokenHandler)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _tokenHandler = tokenHandler;
        }

        public string CreateJwtToken(UserModel user, List<string> roles)
        {
            try
            {
                if (user == null)
                    return String.Empty;
                if (roles == null)
                    return String.Empty;

                var claims = new List<Claim>
                {
                    new ("userId", user.Id),
                };

                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

                var key = _configuration[JwtKey];
                if (string.IsNullOrEmpty(key))
                    throw new InvalidOperationException("Jwt key is not configured");

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    _configuration[JwtIssuer],
                    _configuration[JwtAudience],
                    claims,
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: credentials
                );

                return _tokenHandler.WriteToken(token);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating JWT token: {ex.Message}");
                return string.Empty;
            }
        }

        public string? ParseTokenToUserId(string input)
        {
            try
            {
                if (!input.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    return null;
                string token = input["Bearer ".Length..].Trim();

                var decodedToken = _tokenHandler.ReadJwtToken(token);
                if(decodedToken == null)
                    return null;

                var userIdClaim = decodedToken.Claims.FirstOrDefault(c => c.Type == "userId");
                if(userIdClaim == null)
                    return null;

                string? userId = userIdClaim?.Value;
                if (string.IsNullOrEmpty(userId))
                    return null;

                return userId;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return null;
        }
    }
}
