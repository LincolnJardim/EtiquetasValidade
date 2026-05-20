using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etiquetas.Application.DTOs
{
    public class EtiquetaResponseDto
    {
        public string NomeProduto { get; set; }

        public DateTime DataProducao { get; set; }

        public DateTime DataValidade { get; set; }
    }
}