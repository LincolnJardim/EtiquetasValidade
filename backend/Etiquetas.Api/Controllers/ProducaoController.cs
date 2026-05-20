using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Etiquetas.Domain.Entities;
using Etiquetas.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Etiquetas.Application.Services;

namespace Etiquetas.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProducaoController : ControllerBase
    {
        private readonly ProducaoService _service;

        public ProducaoController(ProducaoService service)
        {
            _service = service;
        }

        
        [HttpPost]
        public IActionResult CriarProducao(int produtoId, DateTime dataFabricacao, int quantidade)
        {
            var producao = _service.CriarProducao(produtoId, dataFabricacao, quantidade);
            return Ok(producao);
        }

        [HttpGet("obterProducaoPorId{id}")]
        public IActionResult ObterProducaoPorId(int id)
        {
            var producao = _service.ObterProduçoesPorId(id);

            if (producao == null)
                return NotFound();

            return Ok(producao);
        }

        [HttpPut("atualizarProducao{id}")]
        public IActionResult AtualizarProducao(int id, Producao producao)
        {
            var producaoAtualizado = _service.AtualizarProducao(id, producao);

            if (producaoAtualizado == null)
                return NotFound();

            return Ok(producaoAtualizado);
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarProducao(int id)
        {
            var producaoBanco = _service.DeletarProducao(id);

            if (producaoBanco == null)
                return NotFound();

            return NoContent();
        }

        [HttpGet("{id}")]
        public IActionResult GerarEtiqueta(int id)
        {
            var etiquetas = _service.GerarEtiqueta(id);

            if (etiquetas == null)
                return NotFound();
            
            return Ok(etiquetas);

        }
    }
}