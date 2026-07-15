using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Etiquetas.Application.DTOs;
using Etiquetas.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Etiquetas.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(
            typeof(LoginResponseDto),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            StatusCodes.Status400BadRequest
        )]
        [ProducesResponseType(
            StatusCodes.Status401Unauthorized
        )]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                LoginResponseDto resultado = await _authService.ValidarCredenciaisAsync(loginDto);

                return Ok(resultado);
            }
            catch (UnauthorizedAccessException excecao)
            {
                return Unauthorized(new
                {
                    mensagem = excecao.Message
                });
            }
        }
    }
}