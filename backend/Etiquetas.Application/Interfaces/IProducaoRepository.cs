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

        Producao? ObterProducaoPorId(int id);

        List<Producao> ListarProducoes();

        bool ExisteProducaoParaProduto(
            int produtoId
        );

        void AtualizarProducao(Producao producao);

        void DeletarProducao(Producao producao);
    }
}