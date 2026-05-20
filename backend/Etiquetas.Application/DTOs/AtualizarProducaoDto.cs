using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etiquetas.Application.DTOs
{
    public class AtualizarProducaoDto
    {
        public int Id { get; set; }
        public DateTime DataFabricacao { get; set; }

        public int QuantidadeEtiquetas { get; set; }
    }
}