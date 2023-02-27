using ASPNETCoreWebAPIJWT.Model;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace ASPNETCoreWebAPIJWT.Repository
{
    public class JWTManagerRepository : IJWTManagerRepository
    {
        private readonly List<User> users = new()
        {
            new User("1","user1","password1"),
            new User("2","user2","password2"),
            new User("3","user3","password3"),
        };

        private readonly IConfiguration iconfiguration;
        public JWTManagerRepository(IConfiguration iconfiguration)
        {
            this.iconfiguration = iconfiguration;
        }
        public Token Authenticate(User user)
        {

            User findedUser = users.Find((u) => u.Name == user.Name && u.Password == user.Password);
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
            return new Token { AccessToken = tokenHandler.WriteToken(token), RefreshToken = tokenHandler.WriteToken(token2) };

        }

        public Token NewAccessToken()
        {       
            // Else we generate JSON Web Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(iconfiguration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {         
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };         
            var accessToken = tokenHandler.CreateToken(tokenDescriptor);

            return new Token { AccessToken = tokenHandler.WriteToken(accessToken), RefreshToken = ""};

        }

        public User GetUserById(string userId)
        {
            return users.Find(u => u.Id == userId);
        }


       public bool ValidateToken(string authToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();

            SecurityToken validatedToken;
            IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
            return true;
        }


        private static TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = false, // Because there is no expiration in the generated token
                ValidateAudience = false, // Because there is no audiance in the generated token
                ValidateIssuer = false,   // Because there is no issuer in the generated token
                ValidIssuer = "Sample",
                ValidAudience = "Sample",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("This is my supper secret key for jwt")) // The same key as the one that generate the token
            };
        }


    }
}
