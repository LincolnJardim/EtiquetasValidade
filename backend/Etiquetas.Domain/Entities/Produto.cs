using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Etiquetas.Domain.Entities
{
    public class Produto
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public int DiasValidade { get; set; }

        public int EmpresaId { get; set; }
    }
}