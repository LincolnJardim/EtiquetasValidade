using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Etiquetas.Application.Interfaces;
using Etiquetas.Domain.Entities;

namespace Etiquetas.Application.Services
{
    public class ProdutoService
    {
        private readonly IProdutoRepository _repository;

        public ProdutoService(IProdutoRepository repository)
        {
            _repository = repository;
        }

        public Produto CriarProduto(Produto produto)
        {
            if (string.IsNullOrEmpty(produto.Nome))
                throw new Exception("Nome obrigatório");

            if (produto.DiasValidade <= 0)
                throw new Exception("Validade inválida");

            _repository.Adicionar(produto);

            return produto;
        }

        public Produto? ObterPorId(int id)
        {
            return _repository.ObterPorId(id);
        }

        public List<Produto> ListarTodos()
        {
            return _repository.ListarTodos();
        }
    }
}