using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etiquetas.Application.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;

        public string Tipo { get; set; } = "Bearer";

        public UsuarioResponseDto Usuario { get; set; } = new();
    }
}