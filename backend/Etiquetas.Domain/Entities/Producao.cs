using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etiquetas.Domain.Entities
{
    public class Producao
    {
        public int Id { get; set; }

        /*
            Chave estrangeira que identifica o produto
            relacionado à produção.
        */
        public int ProdutoId { get; set; }

        public Produto Produto { get; set; } = null!;

        public DateTime DataFabricacao { get; set; }

        public DateTime DataValidade { get; set; }

        public int QuantidadeEtiquetas { get; set; }


        public void CalcularValidade()
        {
            DataValidade =
                DataFabricacao.AddDays(
                    Produto.DiasValidade
                );
        }
    }
}