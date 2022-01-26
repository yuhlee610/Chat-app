using Backend.IRepository;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Repository
{
    public class IdentityRepository : IIdentityRepository
    {
        public string Authenticate(string email, string guid)
        {
            List<string> roles = new List<string>() { "user" };
            return GenerateAccessToken(email, guid, roles.ToArray());
        }

        private string GenerateAccessToken(string email, string guid, string[] roles)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("secretsecretsecret00900"));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, guid),
                new Claim(ClaimTypes.Name, email)
            };

            claims = claims.Concat(roles.Select(role => new Claim(ClaimTypes.Role, role))).ToList();

            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                "issuer",
                "audience",
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
