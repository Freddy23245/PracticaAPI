using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AbmAPI.Entidades
{
    public class JwtManager
    {
        private const string ClaveSecreta = "clave_secreta_aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"; // Cambia esto con una clave segura

        public string GenerarToken(Login usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var clave = Encoding.ASCII.GetBytes(ClaveSecreta);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, usuario.usuario.ToString()),
                new Claim(ClaimTypes.Name, usuario.password)
            }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(clave), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
