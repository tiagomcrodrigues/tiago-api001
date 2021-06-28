using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
 using api001.Models;
 using Microsoft.IdentityModel.Tokens;

namespace api001.Services
{
    public static class tokenService
    {

        public static string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Settings.secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(new Claim[]
                {
                     new Claim(ClaimTypes.Name,user.Username.ToString()),
                     new Claim(ClaimTypes.Role,user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);


        }
    }
}

        





