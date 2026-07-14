using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etiquetas.Application.DTOs
{
    public class UsuarioResponseDto
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public bool Ativo { get; set; }
    }
}