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

namespace Etiquetas.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutoService _service;

        public ProdutoController(ProdutoService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Criar(CriarProdutoDto produtoDto)
        {
            var novoProduto = _service.CriarProduto(produtoDto);
            return Ok(novoProduto);
        }

        [HttpGet("listarProdutosCadastrados")]
        public IActionResult ListarTodos()
        {
            var listaProdutos = _service.ListarTodos();

            return Ok(listaProdutos);
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
        public IActionResult AtualizarProduto(AtualizarProdutoDto atualizarProdutoDto)
        {
            var produtoAtualizado = _service.AtualizarProduto(atualizarProdutoDto);

            if (atualizarProdutoDto == null)
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