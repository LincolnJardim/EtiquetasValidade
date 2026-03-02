using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etiquetas.Domain.Entities
{
    public class Producao
    {
        public int Id { get; set; }

        public Produto Produto { get; set; }

        public DateTime DataFabricacao { get; set; }

        public DateTime DataValidade { get; set; }

        public int QuantidadeEtiquetas { get; set; }

        public void CalcularValidade(Produto Produto, DateTime DataFabricação)
        {
            DataValidade = DataFabricação.AddDays(Produto.DiasValidade);
        }
    }
}