using E_BOOKLIBRARY.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_BOOKLIBRARY.Services
{
    public class JWTService : IJWTService
    {
        private readonly IConfiguration _config;

        public JWTService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(AppUser user, List<string> userRoles, IList<Claim> claims)
        {
            // add claims


            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claims.Add(new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"));
            claims.Add(new Claim(ClaimTypes.Email, $"{user.Email}"));


            // add roles to claims
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // set secret key
            var symetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("JWT:SecretKey").Value));

            // define security token descritpor
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Today.AddDays(1),
                SigningCredentials = new SigningCredentials(symetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };


            // create token
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            return jwtSecurityTokenHandler.WriteToken(token);
        }
    }
}
