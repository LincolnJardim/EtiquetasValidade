using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Etiquetas.Application.DTOs;
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

        public Produto CriarProduto(CriarProdutoDto produtoDto)
        {
            if (string.IsNullOrEmpty(produtoDto.Nome))
                throw new Exception("Nome obrigatório");

            if (produtoDto.DiasValidade <= 0)
                throw new Exception("Validade inválida");
            
            var novoProduto = new Produto
            {
                Nome = produtoDto.Nome,
                DiasValidade = produtoDto.DiasValidade
            };

            _repository.Adicionar(novoProduto);

            return novoProduto;
        }

        public Produto? ObterPorId(int id)
        {
            return _repository.ObterPorId(id);
        }

        public List<Produto> ListarTodos()
        {
            return _repository.ListarTodos();
        }

        public Produto AtualizarProduto(AtualizarProdutoDto atualizarProdutoDto)
        {
            var produtoBanco = _repository.ObterPorId(atualizarProdutoDto.id);

            if (produtoBanco == null)
                throw new Exception("Produto não encontrado");
            
            produtoBanco.Nome = atualizarProdutoDto.Nome;
            produtoBanco.DiasValidade = atualizarProdutoDto.DiasValidade;

            _repository.AtualizarProduto(produtoBanco);

            return produtoBanco;
        }

        public Produto DeletarProduto(int id)
        {
            var produtoBanco = _repository.ObterPorId(id);

            if (produtoBanco == null)
                throw new Exception("Produto não encontrado");
            
            _repository.DeletarProduto(produtoBanco);

            return produtoBanco;
        }
    }
}