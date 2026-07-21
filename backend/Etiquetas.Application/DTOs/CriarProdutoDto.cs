using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Etiquetas.Application.DTOs
{
    public class CriarProdutoDto
    {
        [Required(ErrorMessage = "Informe o nome do produto.")]
        [StringLength(
            100,
            MinimumLength = 2,
            ErrorMessage = "O nome do produto deve possuir entre 2 e 100 caracteres."
        )]
        public string Nome { get; set; } = string.Empty;


        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "A validade deve ser maior que zero."
        )]
        public int DiasValidade { get; set; }
    }
}