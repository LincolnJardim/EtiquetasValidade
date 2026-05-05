using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Etiquetas.Domain.Entities;

namespace Etiquetas.Application.Interfaces
{
    public interface IProducaoRepository
    {
        void AdicionarProducao(Producao producao);

        Producao? ObterProducaoPorId(int Id);

        List<Producao> ListarProducoes();
    }
}