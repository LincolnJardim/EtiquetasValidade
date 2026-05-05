using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Etiquetas.Application.Interfaces;
using Etiquetas.Domain.Entities;
using Etiquetas.Infrastructure.Data;

namespace Etiquetas.Infrastructure.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly EtiquetaDbContext _context;

        public ProdutoRepository(EtiquetaDbContext context)
        {
            _context = context;
        }

        public void Adicionar(Produto produto)
        {
            _context.Produtos.Add(produto);
            _context.SaveChanges();
        }

        public Produto? ObterPorId(int id)
        {
            return _context.Produtos.Find(id);
        }

        public List<Produto> ListarTodos()
        {
            return _context.Produtos.ToList();
        }

        public void AtualizarProduto(Produto produto)
        {
            _context.Produtos.Update(produto);
            _context.SaveChanges();
        }
    }
}