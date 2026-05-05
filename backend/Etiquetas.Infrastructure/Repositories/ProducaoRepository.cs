using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Etiquetas.Application.Interfaces;
using Etiquetas.Domain.Entities;
using Etiquetas.Infrastructure.Data;

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
            return _context.Producoes.ToList();
            
        }

        public Producao? ObterProducaoPorId(int id)
        {
            return _context.Producoes.Find(id);
        }
    }
}