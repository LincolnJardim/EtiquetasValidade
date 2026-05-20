using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etiquetas.Application.DTOs
{
    public class AtualizarProdutoDto
    {
        public int id { get; set; }

        public string Nome { get; set; }

        public int DiasValidade { get; set; }
        
    }
}