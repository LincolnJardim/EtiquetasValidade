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
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutoService _service;

        public ProdutoController(ProdutoService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Criar(Produto produto)
        {
            var novoProduto = _service.CriarProduto(produto);
            return Ok(novoProduto);
        }

        [HttpGet("obterProdutoPorId{id}")]
        public IActionResult ObterPorId(int id)
        {
            var produto = _service.ObterPorId(id);

            if (produto == null)
                return NotFound();

            return Ok(produto);
        }

        [HttpPut("atualizarProduto{id}")]
        public IActionResult AtualizarProduto(int id, Produto produto)
        {
            var produtoAtualizado = _service.AtualizarProduto(id, produto);

            if (produto == null)
                return NotFound();

            return Ok(produtoAtualizado);
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarProduto(int id)
        {
            var produtoBanco = _service.DeletarProduto(id);

            if (produtoBanco == null)
                return NotFound();

            return NoContent();
        }
    }
}