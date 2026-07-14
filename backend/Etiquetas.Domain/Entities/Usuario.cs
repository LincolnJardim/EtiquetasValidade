using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etiquetas.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }

        public required string Nome { get; set; }

        public required string Email { get; set; }

        public required string SenhaHash { get; set; }

        public bool Ativo { get; set; } = true;
    }
}