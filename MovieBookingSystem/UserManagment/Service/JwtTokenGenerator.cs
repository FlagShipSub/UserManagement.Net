using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserManagment.Configurations;
using UserManagment.Models;
using UserManagment.Service.IService;

namespace UserManagment.Service
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtConfig _jwtConfig;
        public JwtTokenGenerator(IOptions<JwtConfig> jwtConfig)
        {
            _jwtConfig = jwtConfig.Value;
        }
        public string GeneraterToken(ApplicationUser applicationUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key  = Encoding.UTF8.GetBytes(_jwtConfig.Secret);
            var listOfClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email,applicationUser.Email),
                new Claim(JwtRegisteredClaimNames.Name,applicationUser.UserName),
                new Claim(JwtRegisteredClaimNames.Sub,applicationUser.Id),
            };
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Audience = _jwtConfig.Audience,
                Issuer = _jwtConfig.Issuer,
                Subject = new ClaimsIdentity(listOfClaims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
