using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Etiquetas.Domain.Entities;
using Etiquetas.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Etiquetas.Application.Services;
using Etiquetas.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Etiquetas.Application.DTOs;
using Etiquetas.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Etiquetas.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ProducaoController : ControllerBase
    {
        private readonly ProducaoService _service;


        public ProducaoController(
            ProducaoService service
        )
        {
            _service = service;
        }


        // Endpoint responsável por criar uma nova produção.
        [HttpPost]
        public IActionResult CriarProducao(
            [FromBody]
            CriarProducaoDto criarProducaoDto
        )
        {
            try
            {
                var producao =
                    _service.CriarProducao(
                        criarProducaoDto
                    );

                return CreatedAtAction(
                    nameof(ObterProducaoPorId),
                    new { id = producao.Id },
                    producao
                );
            }
            catch (ValidationException erro)
            {
                return BadRequest(new
                {
                    mensagem = erro.Message
                });
            }
            catch (KeyNotFoundException erro)
            {
                return NotFound(new
                {
                    mensagem = erro.Message
                });
            }
        }


        // Endpoint responsável por listar as produções cadastradas.
        [HttpGet("listarProducoesCadastradas")]
        public IActionResult ListarProducoes()
        {
            var listaProducoes =
                _service.ListarProducoes();

            return Ok(listaProducoes);
        }


        // Endpoint responsável por buscar uma produção pelo identificador.
        [HttpGet("obterProducaoPorId{id:int}")]
        public IActionResult ObterProducaoPorId(
            int id
        )
        {
            var producao =
                _service.ObterProducaoPorId(id);

            if (producao == null)
            {
                return NotFound(new
                {
                    mensagem =
                        "Produção não encontrada."
                });
            }

            return Ok(producao);
        }


        // Endpoint responsável por atualizar uma produção existente.
        [HttpPut("atualizarProducao{id:int}")]
        public IActionResult AtualizarProducao(
            int id,
            [FromBody]
            AtualizarProducaoDto atualizarProducaoDto
        )
        {
            try
            {
                var producaoAtualizada =
                    _service.AtualizarProducao(
                        id,
                        atualizarProducaoDto
                    );

                if (producaoAtualizada == null)
                {
                    return NotFound(new
                    {
                        mensagem =
                            "Produção não encontrada."
                    });
                }

                return Ok(producaoAtualizada);
            }
            catch (ValidationException erro)
            {
                return BadRequest(new
                {
                    mensagem = erro.Message
                });
            }
        }


        // Endpoint responsável por excluir uma produção.
        [HttpDelete("{id:int}")]
        public IActionResult DeletarProducao(
            int id
        )
        {
            var producaoExcluida =
                _service.DeletarProducao(id);

            if (!producaoExcluida)
            {
                return NotFound(new
                {
                    mensagem =
                        "Produção não encontrada."
                });
            }

            return NoContent();
        }


        // Endpoint responsável por gerar as etiquetas de uma produção.
        [HttpGet("gerarEtiqueta{id:int}")]
        public IActionResult GerarEtiqueta(int id)
        {
            var etiquetas =
                _service.GerarEtiqueta(id);

            if (etiquetas == null)
            {
                return NotFound(new
                {
                    mensagem =
                        "Produção não encontrada."
                });
            }

            return Ok(etiquetas);
        }
    }
}