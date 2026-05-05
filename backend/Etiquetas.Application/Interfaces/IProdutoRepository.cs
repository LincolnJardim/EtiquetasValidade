using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Etiquetas.Domain.Entities;

namespace Etiquetas.Application.Interfaces
{
    public interface IProdutoRepository
    {
        void Adicionar(Produto produto);
        Produto? ObterPorId(int id);
        List<Produto> ListarTodos();

        void AtualizarProduto(Produto produto);
        
    }
}