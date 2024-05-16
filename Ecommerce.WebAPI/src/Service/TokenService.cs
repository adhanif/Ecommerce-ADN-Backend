
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Ecommerce.Core.src.Entity;
using Ecommerce.Service.src.ServiceAbstract;
using Microsoft.Extensions.Caching.Memory;

namespace Ecommerce.WebAPI.src.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;

        public TokenService(IConfiguration configuration, IMemoryCache cache)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public string GetToken(User foundUser)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, foundUser.Email),
                new Claim(ClaimTypes.NameIdentifier, foundUser.Id.ToString()),
                new Claim(ClaimTypes.Role, foundUser.Role.ToString()),
            };

            // secret key
            var jwtKey = _configuration["Secrets:JwtKey"];
            if (jwtKey is null)
            {
                throw new ArgumentNullException("JwtKey is not found in appsettings.json");
            }
            var securityKey = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)), SecurityAlgorithms.HmacSha256Signature);

            // Token handler
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = securityKey,
                Issuer = _configuration["Secrets:Issuer"], // Who generate the token: webdemo
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            // Store the token with a consistent key
            _cache.Set("JwtToken", token);
            return tokenHandler.WriteToken(token);
        }


        public async Task<string> InvalidateTokenAsync()
        {
            var storedToken = _cache.Get("JwtToken");
            if (storedToken is not null)
            {
                _cache.Remove("JwtToken");
                await Task.CompletedTask;
                return "removed";
            }
            else
            {
                await Task.CompletedTask;
                return "already removed";
            }
        }
    }
}

