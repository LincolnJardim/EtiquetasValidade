using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etiquetas.Domain.Entities
{
    public class Empresa
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public DateTime DataCriacao { get; set; }

        
    }
}