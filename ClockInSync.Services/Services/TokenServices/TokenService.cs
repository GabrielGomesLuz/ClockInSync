using ClockInSync.Repositories.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ClockInSync.Services.TokenServices
{


    public interface ITokenService
    {
        string GenerateToken(User user);

    }

    public class TokenService(IConfiguration configuration) : ITokenService
    {


        private readonly IConfiguration _configuration = configuration;

        public string GenerateToken(User user)
        {
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("Jwt:SecretKey").Value!);

            //Criando objeto manipulador para o token
            var handler = new JwtSecurityTokenHandler();

            var credentials =  new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature);


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GenerateClaims(user),
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddHours(2),
            };

            //Gera um token
            var token = handler.CreateToken(tokenDescriptor);


            //Gera uma string do Token
            return handler.WriteToken(token);

        }


        private static ClaimsIdentity GenerateClaims(User user)
        {

            var ci = new ClaimsIdentity();
            ci.AddClaim(new Claim(ClaimTypes.Name, user.Email));
            ci.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

            ci.AddClaim(new Claim(ClaimTypes.Role, user.Role.ToString()));


            return ci;

        }
    }
}
