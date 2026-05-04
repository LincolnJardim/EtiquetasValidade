using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Etiquetas.Domain.Entities;
using Etiquetas.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace Etiquetas.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly EtiquetaDbContext _context;

        public ProdutoController(EtiquetaDbContext context)
        {
            _context = context;
        }

        [HttpPost("CriarNovoProduto")]
        public IActionResult CriarNovoProduto(Produto produto)
        {
            _context.Add(produto);
            _context.SaveChanges();
            return Ok(produto);
        }

        [HttpGet("{id}")]
        public IActionResult ObterProdutoPorId(int id)
        {
            var produto = _context.Produtos.Find(id);

            if (produto == null)
                return NotFound();
            
            return Ok(produto);
        }
    }
}