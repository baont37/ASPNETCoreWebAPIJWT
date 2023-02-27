using ASPNETCoreWebAPIJWT.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ASPNETCoreWebAPIJWT.Repository
{
    public class JWTManagerRepository : IJWTManagerRepository
    {
        private readonly List<Users> users = new()
        {
            new Users("1","user1","password1"),
            new Users("2","user2","password2"),
            new Users("3","user3","password3"),
        };

        private readonly IConfiguration iconfiguration;
        public JWTManagerRepository(IConfiguration iconfiguration)
        {
            this.iconfiguration = iconfiguration;
        }
        public Tokens Authenticate(Users user)
        {

            Users findedUser = users.Find((u) => u.Name == user.Name && u.Password == user.Password);
            if (findedUser == null)
            {
                return null;
            }

            // Else we generate JSON Web Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(iconfiguration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
              {
             new Claim("user_id", findedUser.Id)
              }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenDescriptor2 = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
             {
             new Claim("user_id", findedUser.Id)
             }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var token2 = tokenHandler.CreateToken(tokenDescriptor2);
            return new Tokens { Token = tokenHandler.WriteToken(token), RefreshToken = tokenHandler.WriteToken(token2) };

        }

        public Users GetUserById(string userId)
        {
            return users.Find(u => u.Id == userId);
        }

    }
}
