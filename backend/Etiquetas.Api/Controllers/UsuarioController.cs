using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Etiquetas.Application.DTOs;
using Etiquetas.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Etiquetas.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioResponseDto>> CriarUsuario([FromBody] CriarUsuarioDto criarUsuarioDto)
        {
            try
            {
                UsuarioResponseDto usuarioCriado = await _usuarioService.CriarNovoUsuarioAsync(criarUsuarioDto);

                return StatusCode(StatusCodes.Status201Created, usuarioCriado
                );
            } catch (InvalidOperationException excecao)
            {
                return Conflict(new
                {
                    mensagem = excecao.Message
                });
            }
        }
    }
}