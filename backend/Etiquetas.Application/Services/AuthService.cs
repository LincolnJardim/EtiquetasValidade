using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Etiquetas.Application.DTOs;
using Etiquetas.Application.Interfaces;
using Etiquetas.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Etiquetas.Application.Services
{
    public class AuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasher<Usuario> _passwordHasher;
        private readonly ITokenService _tokenService;
        public AuthService(
            IUsuarioRepository usuarioRepository,
            IPasswordHasher<Usuario> passwordHasher,
            ITokenService tokenService
        )
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task<LoginResponseDto> ValidarCredenciaisAsync(
    LoginDto loginDto
)
        {
            string emailNormalizado = loginDto.Email.Trim().ToLowerInvariant();

            Usuario? usuario = await _usuarioRepository.BuscarPorEmailAsync(emailNormalizado);

            if (usuario is null || !usuario.Ativo)
            {
                throw new UnauthorizedAccessException("Email ou senha inválidos");
            }

            PasswordVerificationResult resultadoSenha = _passwordHasher.VerifyHashedPassword(
                usuario,
                usuario.SenhaHash,
                loginDto.Senha
            );

            if (resultadoSenha == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException("E-mail ou senha inválidos");
            }

            string token = _tokenService.GerarToken(usuario);

return new LoginResponseDto
{
    Token = token,

    Usuario = new UsuarioResponseDto
    {
        Id = usuario.Id,
        Nome = usuario.Nome,
        Email = usuario.Email,
        Ativo = usuario.Ativo
    }
};
        }
    }
}