using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Etiquetas.Application.Interfaces;
using Etiquetas.Domain.Entities;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Etiquetas.Api.Security
{
    public class JwtTokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GerarToken(Usuario usuario)
        {
            string issuer = _configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException("O emissor do JWT não foi encontrado.");

            string audience = _configuration["Jwt:Audience"] ?? throw new InvalidOperationException("O público do JWT não foi encontrado.");

            string chaveBase64 = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("A chave JWT não foi configurada");

            string? expiracaoConfigurada = _configuration["Jwt:ExpirationMinutes"];

            if (!int.TryParse(expiracaoConfigurada, out int minutosExpiracao) || minutosExpiracao <= 0)
            {
                throw new InvalidOperationException("O tempo de expiração do JWT é inválido.");
            }

            byte[] chaveEmBytes;

            try
            {
                chaveEmBytes = Convert.FromBase64String(chaveBase64);
            }
            catch (FormatException)
            {
                throw new InvalidOperationException("A chave do JWT não possui um formato de Base64 válido");
            }

            if (chaveEmBytes.Length < 32)
            {
                throw new InvalidOperationException("A chave do JWT deve possuir pelo menos 32 bytes.");
            }

            Claim[] claims =
            [
                new Claim("sub", usuario.Id.ToString()),
                new Claim("name", usuario.Nome),
                new Claim("email", usuario.Email),
                new Claim("jti", Guid.NewGuid().ToString())
            ];

            SymmetricSecurityKey chaveSeguranca = new SymmetricSecurityKey(chaveEmBytes);

            SigningCredentials credenciais = new SigningCredentials(chaveSeguranca,SecurityAlgorithms.HmacSha256);

            SecurityTokenDescriptor descricaoToken = new()
            {
                Subject = new ClaimsIdentity(claims),

                Issuer = issuer,

                Audience = audience,

                Expires = DateTime.UtcNow.AddMinutes(
                    minutosExpiracao),

                SigningCredentials = credenciais
            };

            JsonWebTokenHandler tokenHandler = new();

            return tokenHandler.CreateToken(descricaoToken);
        }
    }
}