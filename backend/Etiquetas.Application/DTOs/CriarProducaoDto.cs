using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etiquetas.Application.DTOs
{
    public class CriarProducaoDto
    {
        public int ProdutoId { get; set; }

        public DateTime DataFabricacao { get; set; }

        public int QuantidadeEtiquetas { get; set; }
    }
}