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
        public IActionResult CriarProducao(Producao producao)
        {
            var novaProducao = _service.CriarProducao(producao);
            return Ok(novaProducao);
        }

        [HttpGet("{id}")]
        public IActionResult ObterProducaoPorId(int id)
        {
            var producao = _service.ObterProduçoesPorId(id);

            if (producao == null)
                return NotFound();

            return Ok(producao);
        }
    }
}