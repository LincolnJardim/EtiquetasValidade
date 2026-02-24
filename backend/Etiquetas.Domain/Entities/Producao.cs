using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Etiquetas.Domain.Entities
{
    public class Producao
    {
        public int Id { get; set; }

        public int ProdutoId { get; set; }

        public DateTime DataFabricacao { get; set; }

        public int DataValidade { get; set; }

        public int QuantidadeEtiquetas { get; set; }
    }
}