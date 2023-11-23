using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
         public class TokenService : ITokenService
    {

        private readonly SymmetricSecurityKey _key; //token imzalama keyi

        public TokenService(IConfiguration configuration) 
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]));// yeni simetrik key
        }

        public string CreateToken(AppUser user)
        {
           var claims = new List<Claim> //birden fazla claim olabilir
           {
             new Claim(JwtRegisteredClaimNames.NameId,user.UserName) //claimlerimizi kullanıcı adına eşitlidim
             
           };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature); //keyi imzalayacak algoritma

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}

