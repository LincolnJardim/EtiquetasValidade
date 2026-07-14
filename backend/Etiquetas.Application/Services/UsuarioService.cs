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
    public class UsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasher<Usuario> _passwordHasher;

        public UsuarioService(
            IUsuarioRepository usuarioRepository,
            IPasswordHasher<Usuario> passwordHasher
        )
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<UsuarioResponseDto> CriarNovoUsuarioAsync(
            CriarUsuarioDto criarUsuarioDto
        )
        {
            string nomeNormalizado = criarUsuarioDto.Nome.Trim();

            string emailNormalizado = criarUsuarioDto.Email.Trim().ToLowerInvariant();

            Usuario? usuarioExistente = await _usuarioRepository.BuscarPorEmailAsync(emailNormalizado);

            if (usuarioExistente is not null)
            {
                throw new InvalidOperationException(
                    "Já existe um usuário cadastrado com esse e-mail."
                );
            }

            Usuario novoUsuario = new Usuario
            {
                Nome = nomeNormalizado,
                Email = emailNormalizado,
                SenhaHash = string.Empty,
                Ativo = true
            };

            novoUsuario.SenhaHash = _passwordHasher.HashPassword(
                novoUsuario,
                criarUsuarioDto.Senha
            );

            await _usuarioRepository.CriarNovoUsuarioAsync(novoUsuario);

            return new UsuarioResponseDto
            {
                Id = novoUsuario.Id,
                Nome = novoUsuario.Nome,
                Email = novoUsuario.Email,
                Ativo = novoUsuario.Ativo
            };

        }
    }
}