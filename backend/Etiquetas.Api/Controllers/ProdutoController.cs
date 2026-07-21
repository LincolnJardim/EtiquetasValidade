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
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutoService _service;

        public ProdutoController(ProdutoService service)
        {
            _service = service;
        }


        // Endpoint responsável por criar um novo produto.
        [HttpPost]
        public IActionResult Criar(
            [FromBody] CriarProdutoDto produtoDto
        )
        {
            try
            {
                var novoProduto =
                    _service.CriarProduto(produtoDto);

                return CreatedAtAction(
                    nameof(ObterPorId),
                    new { id = novoProduto.Id },
                    novoProduto
                );
            }
            catch (ValidationException erro)
            {
                return BadRequest(new
                {
                    mensagem = erro.Message
                });
            }
        }


        // Endpoint responsável por listar os produtos cadastrados.
        [HttpGet("listarProdutosCadastrados")]
        public IActionResult ListarTodos()
        {
            var listaProdutos =
                _service.ListarTodos();

            return Ok(listaProdutos);
        }


        // Endpoint responsável por buscar um produto pelo identificador.
        [HttpGet("obterProdutoPorId{id:int}")]
        public IActionResult ObterPorId(int id)
        {
            var produto =
                _service.ObterPorId(id);

            if (produto == null)
            {
                return NotFound(new
                {
                    mensagem = "Produto não encontrado."
                });
            }

            return Ok(produto);
        }


        // Endpoint responsável por atualizar um produto existente.
        [HttpPut("atualizarProduto{id:int}")]
        public IActionResult AtualizarProduto(
            int id,
            [FromBody] AtualizarProdutoDto atualizarProdutoDto
        )
        {
            try
            {
                var produtoAtualizado =
                    _service.AtualizarProduto(
                        id,
                        atualizarProdutoDto
                    );

                if (produtoAtualizado == null)
                {
                    return NotFound(new
                    {
                        mensagem = "Produto não encontrado."
                    });
                }

                return Ok(produtoAtualizado);
            }
            catch (ValidationException erro)
            {
                return BadRequest(new
                {
                    mensagem = erro.Message
                });
            }
        }


        // Endpoint responsável por excluir um produto existente.
        [HttpDelete("{id:int}")]
        public IActionResult DeletarProduto(int id)
        {
            var produtoExcluido =
                _service.DeletarProduto(id);

            if (!produtoExcluido)
            {
                return NotFound(new
                {
                    mensagem = "Produto não encontrado."
                });
            }

            return NoContent();
        }
    }
}