using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Etiquetas.Application.DTOs
{
    public class CriarProducaoDto
    {
        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "Selecione um produto válido."
        )]
        public int ProdutoId { get; set; }


        [Required(
            ErrorMessage = "Informe a data de fabricação."
        )]
        public DateTime? DataFabricacao { get; set; }


        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "A quantidade de etiquetas deve ser maior que zero."
        )]
        public int QuantidadeEtiquetas { get; set; }
    }
}