using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Etiquetas.Application.Interfaces;
using Etiquetas.Domain.Entities;
using Etiquetas.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Etiquetas.Infrastructure.Repositories
{
    public class ProducaoRepository
        : IProducaoRepository
    {
        private readonly EtiquetaDbContext _context;


        public ProducaoRepository(
            EtiquetaDbContext context
        )
        {
            _context = context;
        }


        public void AdicionarProducao(
            Producao producao
        )
        {
            _context.Producoes.Add(producao);
            _context.SaveChanges();
        }


        public List<Producao> ListarProducoes()
        {
            return _context.Producoes
                .Include(producao =>
                    producao.Produto
                )
                .ToList();
        }


        public Producao? ObterProducaoPorId(
            int id
        )
        {
            return _context.Producoes
                .Include(producao =>
                    producao.Produto
                )
                .FirstOrDefault(
                    producao =>
                        producao.Id == id
                );
        }


        /*
            Verifica sem carregar toda a lista se existe
            alguma produção relacionada ao produto.
        */
        public bool ExisteProducaoParaProduto(
            int produtoId
        )
        {
            return _context.Producoes.Any(
                producao =>
                    producao.ProdutoId == produtoId
            );
        }


        public void AtualizarProducao(
            Producao producao
        )
        {
            _context.Producoes.Update(producao);
            _context.SaveChanges();
        }


        public void DeletarProducao(
            Producao producao
        )
        {
            _context.Producoes.Remove(producao);
            _context.SaveChanges();
        }
    }
}