using BoggleApp.Game.Setup;
using BoggleApp.Shared;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BoggleApp.Server.Helpers
{
    public static class TokenGenerator
    {
        public static string Secret => "F65D2CB6-9A61-4FD5-BA75-7A276140C21D";
        public static SymmetricSecurityKey Key => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));

        public static string GenerateToken(User user)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Sid, user.Id)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256Signature)
            };


            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

