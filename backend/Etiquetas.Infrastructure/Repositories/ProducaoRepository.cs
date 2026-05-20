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
    public class ProducaoRepository : IProducaoRepository
    {
        private readonly EtiquetaDbContext _context;

        public ProducaoRepository(EtiquetaDbContext context)
        {
            _context = context;
        }
        public void AdicionarProducao(Producao producao)
        {
            _context.Producoes.Add(producao);
            _context.SaveChanges();
        }

        public List<Producao> ListarProducoes()
        {
            return _context.Producoes
                .Include(p => p.Produto)
                .ToList();
        }

        public Producao? ObterProducaoPorId(int id)
        {
            return _context.Producoes
                .Include(p => p.Produto)
                .FirstOrDefault(p => p.Id == id);
        }

        public void AtualizarProducao(Producao producao)
        {
            _context.Producoes.Update(producao);
            _context.SaveChanges();
        }

        public void DeletarProducao(Producao producao)
        {
            _context.Producoes.Remove(producao);
            _context.SaveChanges();
        }

    }
}