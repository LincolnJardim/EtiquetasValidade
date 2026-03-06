using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etiquetas.Domain.Entities
{
    public class Etiqueta
    {
        public int Id { get; set; }

        public string Produto { get; set; }

        public DateTime DataProducao { get; set; }

        public DateTime DataValidade { get; set; }

        public string Lote { get; set; }
    }
}
