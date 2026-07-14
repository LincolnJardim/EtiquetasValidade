using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Etiquetas.Application.DTOs
{
    public class CriarUsuarioDto
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(
            100,
            MinimumLength = 2,
            ErrorMessage = "O nome deve possuir entre 2 e 100 caracteres."
        )]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Informe um endereço de e-mail válido.")]
        [StringLength(
            150,
            ErrorMessage = "O e-mail deve possuir no máximo 150 caracteres."
        )]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(
            100,
            MinimumLength = 8,
            ErrorMessage = "A senha deve possuir entre 8 e 100 caracteres."
        )]
        public string Senha { get; set; } = string.Empty;
    }
}